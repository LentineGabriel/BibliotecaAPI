#region USINGS
using BibliotecaAPI.Context;
using BibliotecaAPI.DTOs.Mappings;
using BibliotecaAPI.Filters;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
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
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(op =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    op.IncludeXmlComments(xmlPath);
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
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
#endregion
#endregion

#region APP
var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
#endregion