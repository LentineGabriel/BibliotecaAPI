using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.AutoresFiltro;
using BibliotecaAPI.Pagination.CategoriasFiltro;
using X.PagedList;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface IAutorRepositorio : IRepositorio<Autor>
{
    Task<IPagedList<Autor>> GetAutoresAsync(AutoresParameters autoresParameters);
    Task<IPagedList<Autor>> GetAutoresFiltrandoPeloNome(AutoresFiltroNome autoresFiltroNome);
    Task<IPagedList<Autor>> GetAutoresFiltrandoPelaNacionalidade(AutoresFiltroNacionalidade autoresFiltroNacionalidade);
}
