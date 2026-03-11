using BibliotecaAPI.DTOs.LivrosDTOs;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface IBuscarLivro
{
    Task<IPagedList<LivrosDTOResponse>> BuscarLivroAsync(string termo, int page, int pageSize);
}
