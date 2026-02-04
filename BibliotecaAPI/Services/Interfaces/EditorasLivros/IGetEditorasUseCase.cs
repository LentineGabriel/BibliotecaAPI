using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Pagination.EditorasFiltro;
using BibliotecaAPI.Models;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IGetEditorasUseCase
{
    Task<IEnumerable<EditorasDTOResponse>> GetAsync();
    Task<EditorasDTOResponse> GetByIdAsync(int id);
    Task<IPagedList<Editoras>> GetPaginationAsync(EditorasParameters editorasParameters);
    Task<IPagedList<Editoras>> GetFilterNamePaginationAsync(EditorasFiltroNome editorasFiltroNome);
    Task<IPagedList<Editoras>> GetFilterNationalityPaginationAsync(EditorasFiltroPaisOrigem editorasFiltroPaisOrigem);
}
