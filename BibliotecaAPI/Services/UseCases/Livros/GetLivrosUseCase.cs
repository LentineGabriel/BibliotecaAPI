using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Pagination.LivrosFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Livros;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.Livros;
public class GetLivrosUseCase : IGetLivrosUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public GetLivrosUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<LivrosDTOResponse>> GetAsync()
    {
        var livros = await _uof.LivrosRepositorio.GetLivroCompletoAsync();
        return _mapper.Map<IEnumerable<LivrosDTOResponse>>(livros);
    }

    public async Task<LivrosDTOResponse> GetByIdAsync(int id)
    {
        var livro = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        return _mapper.Map<LivrosDTOResponse>(livro);
    }

    public async Task<IPagedList<Models.Livros?>> GetPaginationAsync(LivrosParameters livrosParameters) => await _uof.LivrosRepositorio.GetLivrosAsync(livrosParameters);
    public async Task<IPagedList<Models.Livros?>> GetFilterByNameAsync(LivrosFiltroNome livrosFiltroNome) => await _uof.LivrosRepositorio.GetLivrosFiltrandoPeloNome(livrosFiltroNome);
    public async Task<IPagedList<Models.Livros?>> GetFilterByAutorAsync(LivrosFiltroAutor livrosFiltroAutor) => await _uof.LivrosRepositorio.GetLivrosFiltrandoPeloAutor(livrosFiltroAutor);
    public async Task<IPagedList<Models.Livros?>> GetFilterByEditoraAsync(LivrosFiltroEditora livrosFiltroEditora) => await _uof.LivrosRepositorio.GetLivrosFiltrandoPelaEditora(livrosFiltroEditora);
    public async Task<IPagedList<Models.Livros?>> GetFilterByAnoPublicacaoAsync(LivrosFiltroAnoPublicacao livrosFiltroAnoPublicacao) => await _uof.LivrosRepositorio.GetLivrosFiltrandoPorAnoPublicacao(livrosFiltroAnoPublicacao);
    public async Task<IPagedList<Models.Livros?>> GetFilterByCategoriaAsync(LivrosFiltroCategoria livrosFiltroCategoria) => await _uof.LivrosRepositorio.GetLivrosFiltrandoPorCategoria(livrosFiltroCategoria);
}
