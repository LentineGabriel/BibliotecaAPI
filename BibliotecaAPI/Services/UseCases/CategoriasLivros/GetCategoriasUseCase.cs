using AutoMapper;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Pagination.AutoresFiltro;
using BibliotecaAPI.Pagination.CategoriasFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Categorias;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.CategoriasLivros;

public class GetCategoriasUseCase : IGetCategoriasUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public GetCategoriasUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<CategoriasDTOResponse>> GetAllAsync()
    {
        var categorias = await _uof.CategoriaLivrosRepositorio.GetAllAsync();
        if(categorias == null || !categorias.Any()) throw new ArgumentNullException("Categorias não encontradas. Por favor, tente novamente!");

        return _mapper.Map<IEnumerable<CategoriasDTOResponse>>(categorias);
    }

    public async Task<CategoriasDTOResponse> GetByIdAsync(int id)
    {
        var categoriaId = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(categoriaId == null) throw new KeyNotFoundException($"Categoria por ID = {id} não encontrado. Por favor, tente novamente!");

        return _mapper.Map<CategoriasDTOResponse>(categoriaId);
    }

    public async Task<IPagedList<Models.Categorias>> GetPaginationAsync(CategoriasParameters categoriaParameters) => await _uof.CategoriaLivrosRepositorio.GetCategoriasAsync(categoriaParameters);

    public async Task<IPagedList<Models.Categorias>> GetFilterNamePaginationAsync(CategoriasFiltroNome categoriasFiltroNome) => await _uof.CategoriaLivrosRepositorio.GetCategoriasFiltrandoPeloNome(categoriasFiltroNome);
}
