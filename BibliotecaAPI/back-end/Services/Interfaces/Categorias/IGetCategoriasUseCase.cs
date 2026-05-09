using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Pagination.CategoriasFiltro;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface IGetCategoriasUseCase
{
    Task<IEnumerable<CategoriasDTOResponse>> GetAllAsync();
    Task<CategoriasDTOResponse> GetByIdAsync(int id);
    Task<IPagedList<Models.Categorias>> GetPaginationAsync(CategoriasParameters categoriaParameters);
    Task<IPagedList<Models.Categorias>> GetFilterNamePaginationAsync(CategoriasFiltroNome categoriasFiltroNome);
}
