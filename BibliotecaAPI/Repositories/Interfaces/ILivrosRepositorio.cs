using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface ILivrosRepositorio : IRepositorio<Livros>
{
    Task<IEnumerable<Livros>> GetLivrosComAutorAsync();
    Task<Livros?> GetLivroComAutorAsync(int id);
}
