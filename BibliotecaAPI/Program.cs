#region USINGS
using BibliotecaAPI.Context;
using BibliotecaAPI.DTOs.Mappings;
using BibliotecaAPI.Repositories;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
#endregion

var builder = WebApplication.CreateBuilder(args);

#region SERVICES
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

#region DATABASE & DI
// DATABASE
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(op => op.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// DI
builder.Services.AddScoped(typeof(IRepositorio<>) , typeof(Repositorio<>));
builder.Services.AddScoped<IUnityOfWork , UnityOfWork>();
builder.Services.AddScoped<ILivrosRepositorio , LivrosRepositorio>();
builder.Services.AddAutoMapper(cfg => { } , typeof(MappingProfile));
#endregion
#endregion

#region APP
var app = builder.Build();
if (app.Environment.IsDevelopment())
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