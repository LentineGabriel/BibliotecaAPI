using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Autores;

namespace BibliotecaAPI.Services.UseCases.Autores;
public sealed class PutAutoresUseCase : IPutAutoresUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PutAutoresUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<AutorDTOResponse> PutAsync(AutorDTORequest autorDTO)
    {
        var autor = _mapper.Map<Autor>(autorDTO);
        
        var autorExistente = _uof.AutorRepositorio.Update(autor);
        await _uof.CommitAsync();

        return _mapper.Map<AutorDTOResponse>(autorExistente);
    }
}
