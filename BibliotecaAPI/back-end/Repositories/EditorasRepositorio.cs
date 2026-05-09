using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.EditorasFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using X.PagedList;

namespace BibliotecaAPI.Repositories;

public class EditorasRepositorio : Repositorio<Editoras>, IEditorasRepositorio
{
    #region CTOR
    public EditorasRepositorio(AppDbContext context) : base(context) { }
    #endregion

    #region METHODS
    public async Task<IPagedList<Editoras>> GetEditorasAsync(EditorasParameters editorasParameters)
    {
        var editoras = await GetAllAsync();
        var editorasOrdenadas = editoras.OrderBy(a => a.IdEditora).AsQueryable();
        var resultado = await editorasOrdenadas.ToPagedListAsync(editorasParameters.PageNumber , editorasParameters.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Editoras>> GetEditorasFiltrandoPeloNome(EditorasFiltroNome editorasFiltroNome)
    {
        var editoras = await GetAllAsync();
        if(!string.IsNullOrEmpty(editorasFiltroNome.Nome))
        {
            var termo = editorasFiltroNome.Nome.ToLower();
            editoras = editoras.Where(a => a.NomeEditora != null && a.NomeEditora.ToLower().Contains(termo)).ToList();
        }

        var editorasFiltradas = await editoras.ToPagedListAsync(editorasFiltroNome.PageNumber , editorasFiltroNome.PageSize);
        return editorasFiltradas;
    }

    public async Task<IPagedList<Editoras>> GetEditorasFiltrandoPorPaisDeOrigem(EditorasFiltroPaisOrigem editorasFiltroPaisOrigem)
    {
        var nacionalidades = await GetAllAsync();
        if(!string.IsNullOrEmpty(editorasFiltroPaisOrigem.Nacionalidade))
        {
            var termo = editorasFiltroPaisOrigem.Nacionalidade.ToLower();
            nacionalidades = nacionalidades.Where(a => a.PaisOrigem != null && a.PaisOrigem.ToLower().Contains(termo)).ToList();
        }

        var paisOrigemFiltrados = await nacionalidades.ToPagedListAsync(editorasFiltroPaisOrigem.PageNumber , editorasFiltroPaisOrigem.PageSize);
        return paisOrigemFiltrados;
    }
    #endregion
}
