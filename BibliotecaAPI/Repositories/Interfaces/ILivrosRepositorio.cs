using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.LivrosFiltro;
using X.PagedList;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface ILivrosRepositorio : IRepositorio<Livros>
{
    Task<IEnumerable<Livros?>> GetLivroCompletoAsync();
    Task<Livros?> GetLivroCompletoAsync(int id);
    Task<IPagedList<Livros?>> GetLivrosAsync(LivrosParameters livrosParameters);
    Task<IPagedList<Livros?>> GetLivrosFiltrandoPeloNome(LivrosFiltroNome livrosFiltroNome);
    Task<IPagedList<Livros?>> GetLivrosFiltrandoPeloAutor(LivrosFiltroAutor livrosFiltroAutor);
    Task<IPagedList<Livros?>> GetLivrosFiltrandoPelaEditora(LivrosFiltroEditora livrosFiltroEditora);
    Task<IPagedList<Livros?>> GetLivrosFiltrandoPorAnoPublicacao(LivrosFiltroAnoPublicacao livrosFiltroAnoPublicacao);
    Task<IPagedList<Livros?>> GetLivrosFiltrandoPorCategoria(LivrosFiltroCategoria livrosFiltroCategoria);
}
