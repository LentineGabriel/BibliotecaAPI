using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;

namespace BibliotecaAPI.Repositories;
public class LivrosRepositorio : Repositorio<Livros>, ILivrosRepositorio
{
    public LivrosRepositorio(AppDbContext context) : base(context) { }
}
