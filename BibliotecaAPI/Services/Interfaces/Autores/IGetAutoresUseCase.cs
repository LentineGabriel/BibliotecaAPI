using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.AutoresFiltro;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Autores;
public interface IGetAutoresUseCase
{
    Task<IEnumerable<AutorDTOResponse>> GetAllAsync();
    Task<AutorDTOResponse> GetByIdAsync(int id);
    Task<IPagedList<Autor>> GetPaginationAsync(AutoresParameters autoresParameters);
    Task<IPagedList<Autor>> GetFilterNamePaginationAsync(AutoresFiltroNome autoresFiltroNome);
    Task<IPagedList<Autor>> GetFilterNationalityPaginationAsync(AutoresFiltroNacionalidade autoresFiltroNacionalidade);
}
