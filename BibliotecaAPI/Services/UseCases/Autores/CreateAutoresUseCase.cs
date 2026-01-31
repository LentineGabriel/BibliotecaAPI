using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Autores;

namespace BibliotecaAPI.Services.UseCases.Autores;

public sealed class CreateAutoresUseCase : ICreateAutoresUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public CreateAutoresUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<AutorDTOResponse> CreateAsync(AutorDTORequest autorDTO)
    {
        var autorNovo = _mapper.Map<Autor>(autorDTO);

        var autorCriado = _uof.AutorRepositorio.Create(autorNovo);
        await _uof.CommitAsync();

        return _mapper.Map<AutorDTOResponse>(autorCriado);
    }
}
