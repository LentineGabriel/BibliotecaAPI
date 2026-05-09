using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.EditorasFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EditorasLivros;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.EditorasLivros;

public class GetEditorasUseCase : IGetEditorasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public GetEditorasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<EditorasDTOResponse>> GetAsync()
    {
        var editoras = await _uof.EditorasRepositorio.GetAllAsync();
        return _mapper.Map<IEnumerable<EditorasDTOResponse>>(editoras);
    }

    public async Task<EditorasDTOResponse> GetByIdAsync(int id)
    {
        var editora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        return _mapper.Map<EditorasDTOResponse>(editora);
    }

    public async Task<IPagedList<Editoras>> GetPaginationAsync(EditorasParameters editorasParameters)
        => await _uof.EditorasRepositorio.GetEditorasAsync(editorasParameters);

    public async Task<IPagedList<Editoras>> GetFilterNamePaginationAsync(EditorasFiltroNome editorasFiltroNome)
        => await _uof.EditorasRepositorio.GetEditorasFiltrandoPeloNome(editorasFiltroNome);

    public async Task<IPagedList<Editoras>> GetFilterNationalityPaginationAsync(EditorasFiltroPaisOrigem editorasFiltroPaisOrigem)
        => await _uof.EditorasRepositorio.GetEditorasFiltrandoPorPaisDeOrigem(editorasFiltroPaisOrigem);
}
