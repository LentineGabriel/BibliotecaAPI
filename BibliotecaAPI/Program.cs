#region USINGS
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BibliotecaAPI.Context;
using BibliotecaAPI.DTOs.Mappings;
using BibliotecaAPI.Filters;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services;
using BibliotecaAPI.Services.Interfaces;
using BibliotecaAPI.Settings;
using BibliotecaAPI.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
#endregion

#region CULTURE PT-BR
var culture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
#endregion

var builder = WebApplication.CreateBuilder(args);

#region SERVICES
builder.Services.AddControllers(op =>
{
    op.Filters.Add(typeof(ApiExceptionFilter));
}).AddNewtonsoftJson(op =>
{
    op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).AddJsonOptions(op =>
{
    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// versionamento do projeto
builder.Services.AddApiVersioning(op =>
{
    op.DefaultApiVersion = new ApiVersion(1 , 0);
    op.AssumeDefaultVersionWhenUnspecified = true;
    op.ReportApiVersions = true;
    op.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(op =>
{
    op.GroupNameFormat = "'v'VVV";
    op.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions> , ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(c =>
{
    // comentários XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory , xmlFile);
    c.IncludeXmlComments(xmlPath);

    // JWT Bearer
    c.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme
    {
        Name = "Authorization" ,
        Type = SecuritySchemeType.Http ,
        Scheme = "bearer" ,
        BearerFormat = "JWT" ,
        In = ParameterLocation.Header ,
        Description = "Informe o token no formato: {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("AdminsAndUsers" , policy => policy.RequireRole("Admins" , "Users"));
    op.AddPolicy("AdminsOnly" , policy => policy.RequireRole("Admins"));
    op.AddPolicy("UsersOnly" , policy => policy.RequireRole("Users"));
});

#region DATABASE & DI
// DATABASE
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(op => op.UseMySql(connectionString , ServerVersion.AutoDetect(connectionString)));

// DI
builder.Services.AddScoped(typeof(IRepositorio<>) , typeof(Repositorio<>));
builder.Services.AddScoped<IUnityOfWork , UnityOfWork>();
builder.Services.AddScoped<ILivrosRepositorio , LivrosRepositorio>();
builder.Services.AddScoped<IAutorRepositorio , AutorRepositorio>();
builder.Services.AddScoped<IEditorasRepositorio , EditorasRepositorio>();
builder.Services.AddScoped<ICategoriaLivrosRepositorio , CategoriaLivrosRepositorio>();
builder.Services.AddAutoMapper(cfg => { } , typeof(MappingProfile));
builder.Services.AddIdentity<ApplicationUser , IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<ITokenService , TokenService>();
#endregion

#region JWT TOKEN
var jwtSection = builder.Configuration.GetSection("JWT");

// obtém a chave do ambiente
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET não definida!");

builder.Services.Configure<JwtSettings>(op =>
{
    jwtSection.Bind(op);
    op.SecretKey = secretKey;
});

// validando tudo na inicialização
builder.Services.AddOptions<JwtSettings>().Validate(x => !string.IsNullOrWhiteSpace(x.SecretKey) &&
                                                   !string.IsNullOrWhiteSpace(x.ValidIssuer) &&
                                                   !string.IsNullOrWhiteSpace(x.ValidAudience) &&
                                                   x.TokenValidityInMinutes > 0 , "Configuração JWT inválida!").ValidateOnStart();

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.SaveToken = true;
    op.RequireHttpsMetadata = false;
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true ,
        ValidateAudience = true ,
        ValidateLifetime = true ,
        ValidateIssuerSigningKey = true ,
        ClockSkew = TimeSpan.Zero ,
        ValidIssuer = jwtSection["ValidIssuer"] ,
        ValidAudience = jwtSection["ValidAudience"] ,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
#endregion
#endregion

#region APP
var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(op =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach(var desc in provider.ApiVersionDescriptions)
        {
            op.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json" , $"BibliotecaAPI {desc.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion