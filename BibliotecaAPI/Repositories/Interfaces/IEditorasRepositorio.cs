using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.EditorasFiltro;
using X.PagedList;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface IEditorasRepositorio : IRepositorio<Editoras>
{
    Task<IPagedList<Editoras>> GetEditorasAsync(EditorasParameters editorasParameters);
    Task<IPagedList<Editoras>> GetEditorasFiltrandoPeloNome(EditorasFiltroNome editorasFiltroNome);
    Task<IPagedList<Editoras>> GetEditorasFiltrandoPorPaisDeOrigem(EditorasFiltroPaisOrigem editorasFiltroPaisOrigem);
}
