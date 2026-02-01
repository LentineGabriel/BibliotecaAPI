using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Categorias;

namespace BibliotecaAPI.Services.UseCases.CategoriasLivros;

public class PutCategoriasUseCase : IPutCategoriasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PutCategoriasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<CategoriasDTOResponse> PutAsync(CategoriasDTORequest categoriaDTO)
    {
        var categoria = _mapper.Map<Models.Categorias>(categoriaDTO);
        var categoriaExistente = _uof.CategoriaLivrosRepositorio.Update(categoria);
        await _uof.CommitAsync();

        return _mapper.Map<CategoriasDTOResponse>(categoriaExistente);
    }
}
