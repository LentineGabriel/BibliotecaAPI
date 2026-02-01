using AutoMapper;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Categorias;

namespace BibliotecaAPI.Services.UseCases.CategoriasLivros;

public class CreateCategoriasUseCase : ICreateCategoriasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public CreateCategoriasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<CategoriasDTOResponse> PostAsync(CategoriasDTORequest categoriasDTO)
    {
        var categoriaNova = _mapper.Map<Categorias>(categoriasDTO);

        var categoriaCriada = _uof.CategoriaLivrosRepositorio.Create(categoriaNova);
        await _uof.CommitAsync();

        return _mapper.Map<CategoriasDTOResponse>(categoriaCriada);
    }
}
