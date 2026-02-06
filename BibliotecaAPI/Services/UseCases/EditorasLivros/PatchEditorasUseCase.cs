using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EditorasLivros;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Newtonsoft.Json;

namespace BibliotecaAPI.Services.UseCases.EditorasLivros;

public class PatchEditorasUseCase : IPatchEditorasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PatchEditorasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<EditorasDTOResponse> PatchAsync(int id , JsonPatchDocument<EditorasDTORequest> patchDoc)
    {
        if(patchDoc == null) throw new JsonException("Nenhuma opção foi enviada para atualizar parcialmente.");

        var editora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        if(editora == null) throw new NullReferenceException($"Editora por ID = {id} não encontrada. Por favor, tente novamente!");

        var editoraDTO = _mapper.Map<EditorasDTORequest>(editora);
        patchDoc.ApplyTo(editoraDTO);

        _mapper.Map(editoraDTO , editora);
        _uof.EditorasRepositorio.Update(editora);
        await _uof.CommitAsync();

        return _mapper.Map<EditorasDTOResponse>(editora);
    }
}
