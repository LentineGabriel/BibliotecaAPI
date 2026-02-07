using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Pagination.LivrosFiltro;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface IGetLivrosUseCase
{
    Task<IEnumerable<LivrosDTOResponse>> GetAsync();
    Task<LivrosDTOResponse> GetByIdAsync(int id);
    Task<IPagedList<Models.Livros?>> GetPaginationAsync(LivrosParameters livrosParameters);
    Task<IPagedList<Models.Livros?>> GetFilterByNameAsync(LivrosFiltroNome livrosFiltroNome);
    Task<IPagedList<Models.Livros?>> GetFilterByAutorAsync(LivrosFiltroAutor livrosFiltroAutor);
    Task<IPagedList<Models.Livros?>> GetFilterByEditoraAsync(LivrosFiltroEditora livrosFiltroEditora);
    Task<IPagedList<Models.Livros?>> GetFilterByAnoPublicacaoAsync(LivrosFiltroAnoPublicacao livrosFiltroAnoPublicacao);
    Task<IPagedList<Models.Livros?>> GetFilterByCategoriaAsync(LivrosFiltroCategoria livrosFiltroCategoria);
}
