using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.AutoresFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using X.PagedList;

namespace BibliotecaAPI.Repositories;
public class AutorRepositorio : Repositorio<Autor>, IAutorRepositorio
{
    #region CTOR
    public AutorRepositorio(AppDbContext context) : base(context) { }
    #endregion

    #region METHODS
    public async Task<IPagedList<Autor>> GetAutoresAsync(AutoresParameters autoresParameters)
    {
        var autores = await GetAllAsync();
        var autoresOrdenados = autores.OrderBy(a => a.IdAutor).AsQueryable();
        var resultado = await autoresOrdenados.ToPagedListAsync(autoresParameters.PageNumber , autoresParameters.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Autor>> GetAutoresFiltrandoPeloNome(AutoresFiltroNome autoresFiltroNome)
    {
        var autores = await GetAllAsync();
        if(!string.IsNullOrEmpty(autoresFiltroNome.Nome))
        {
            var termo = autoresFiltroNome.Nome.ToLower();
            autores = autores.Where(a => a.NomeCompleto != null && a.NomeCompleto.ToLower().Contains(termo)).ToList();
        }
        
        var autoresFiltrados = await autores.ToPagedListAsync(autoresFiltroNome.PageNumber , autoresFiltroNome.PageSize);
        return autoresFiltrados;
    }

    public async Task<IPagedList<Autor>> GetAutoresFiltrandoPelaNacionalidade(AutoresFiltroNacionalidade autoresFiltroNacionalidade)
    {
        var nacionalidades = await GetAllAsync();
        if(!string.IsNullOrEmpty(autoresFiltroNacionalidade.Nacionalidade))
        {
            var termo = autoresFiltroNacionalidade.Nacionalidade.ToLower();
            nacionalidades = nacionalidades.Where(a => a.Nacionalidade != null && a.Nacionalidade.ToLower().Contains(termo)).ToList();
        }

        var autoresFiltrados = await nacionalidades.ToPagedListAsync(autoresFiltroNacionalidade.PageNumber, autoresFiltroNacionalidade.PageSize);
        return autoresFiltrados;
    }
    #endregion
}
