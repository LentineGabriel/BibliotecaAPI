using AutoMapper;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Categorias;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.UseCases.CategoriasLivros;
public class PatchCategoriasUseCase : IPatchCategoriasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PatchCategoriasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<CategoriasDTOResponse> PatchAsync(int id , JsonPatchDocument<CategoriasDTORequest> patchDoc)
    {
        var categoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(categoria == null) throw new KeyNotFoundException($"Categoria com ID {id} não encontrada. Por favor, verifique o ID digitado e tente novamente!");

        var categoriaDTO = _mapper.Map<CategoriasDTORequest>(categoria);
        patchDoc.ApplyTo(categoriaDTO);

        _mapper.Map(categoriaDTO , categoria);
        _uof.CategoriaLivrosRepositorio.Update(categoria);
        await _uof.CommitAsync();

        return _mapper.Map<CategoriasDTOResponse>(categoria);
    }
}
