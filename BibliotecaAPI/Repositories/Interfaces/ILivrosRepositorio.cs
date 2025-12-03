using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface ILivrosRepositorio : IRepositorio<Livros>
{
    Task<IEnumerable<Livros?>> GetLivroCompletoAsync();
    Task<Livros?> GetLivroCompletoAsync(int id);
}
