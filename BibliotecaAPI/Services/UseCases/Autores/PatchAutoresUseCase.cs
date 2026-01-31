using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Autores;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.UseCases.Autores;

public sealed class PatchAutoresUseCase : IPatchAutoresUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PatchAutoresUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<AutorDTOResponse> PatchAsync(int id , JsonPatchDocument<AutorDTORequest> patchDoc)
    {
        var autor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(autor == null) throw new KeyNotFoundException($"Autor com ID {id} não encontrado. Por favor, verifique o ID e tente novamente!");

        var autorDTO = _mapper.Map<AutorDTORequest>(autor);
        patchDoc.ApplyTo(autorDTO);

        _mapper.Map(autorDTO , autor);
        _uof.AutorRepositorio.Update(autor);
        await _uof.CommitAsync();

        return _mapper.Map<AutorDTOResponse>(autor);
    }
}
