using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;

namespace BibliotecaAPI.Repositories;

public class CategoriaLivrosRepositorio : Repositorio<Categorias>, ICategoriaLivrosRepositorio
{
    public CategoriaLivrosRepositorio(AppDbContext context) : base(context) { }
}
