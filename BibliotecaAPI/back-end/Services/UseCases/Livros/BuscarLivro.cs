using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Services.Interfaces.Livros;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.Livros;

public class BuscarLivro : IBuscarLivro
{
    public Task<IPagedList<LivrosDTOResponse>> BuscarLivroAsync(string termo , int page , int pageSize)
    {
        throw new NotImplementedException();
    }
}
