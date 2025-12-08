using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.Categorias;
using X.PagedList;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface ICategoriaLivrosRepositorio : IRepositorio<Categorias>
{
    Task<IPagedList<Categorias>> GetCategoriasAsync(CategoriaParameters categoriaParameters);
    Task<IPagedList<Categorias>> GetCategoriasFiltrandoPeloNome(CategoriasFiltroNome categoriasFiltroNome);
}
