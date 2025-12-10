using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.CategoriasFiltro;
using X.PagedList;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface ICategoriaLivrosRepositorio : IRepositorio<Categorias>
{
    Task<IPagedList<Categorias>> GetCategoriasAsync(CategoriasParameters categoriaParameters);
    Task<IPagedList<Categorias>> GetCategoriasFiltrandoPeloNome(CategoriasFiltroNome categoriasFiltroNome);
}
