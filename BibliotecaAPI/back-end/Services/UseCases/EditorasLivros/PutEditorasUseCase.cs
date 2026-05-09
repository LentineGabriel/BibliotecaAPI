using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EditorasLivros;

namespace BibliotecaAPI.Services.UseCases.EditorasLivros;

public class PutEditorasUseCase : IPutEditorasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PutEditorasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<EditorasDTOResponse> PutAsync(EditorasDTORequest editorasDTO)
    {
        var editora = _mapper.Map<Editoras>(editorasDTO);
        var editoraExistente = _uof.EditorasRepositorio.Update(editora);
        await _uof.CommitAsync();

        return _mapper.Map<EditorasDTOResponse>(editoraExistente);
    }
}
