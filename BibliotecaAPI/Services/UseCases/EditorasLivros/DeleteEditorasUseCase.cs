using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EditorasLivros;

namespace BibliotecaAPI.Services.UseCases.EditorasLivros;

public class DeleteEditorasUseCase : IDeleteEditorasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public DeleteEditorasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<EditorasDTOResponse> DeleteAsync(int id)
    {
        var deletarEditora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        if(deletarEditora == null) throw new NullReferenceException($"Editora não localizada! Verifique o ID digitado");

        var editoraExcluida = _uof.EditorasRepositorio.Delete(deletarEditora);
        await _uof.CommitAsync();

        return _mapper.Map<EditorasDTOResponse>(editoraExcluida);
    }
}
