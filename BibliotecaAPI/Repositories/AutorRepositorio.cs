using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;

namespace BibliotecaAPI.Repositories;
public class AutorRepositorio : Repositorio<Autor>, IAutorRepositorio
{
    public AutorRepositorio(AppDbContext context) : base(context) { }
}
