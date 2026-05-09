using AutoMapper;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Categorias;

namespace BibliotecaAPI.Services.UseCases.CategoriasLivros;

public class DeleteCategoriasUseCase : IDeleteCategoriasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public DeleteCategoriasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<CategoriasDTOResponse> DeleteAsync(int id)
    {
        var deletarCategoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(deletarCategoria == null) throw new KeyNotFoundException("Categoria não localizada! Verifique o ID digitado");

        var categoriaExcluida = _uof.CategoriaLivrosRepositorio.Delete(deletarCategoria);
        await _uof.CommitAsync();

        return _mapper.Map<CategoriasDTOResponse>(categoriaExcluida);
    }
}
