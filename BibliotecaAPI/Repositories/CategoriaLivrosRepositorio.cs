using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.Categorias;
using BibliotecaAPI.Repositories.Interfaces;
using X.PagedList;

namespace BibliotecaAPI.Repositories;

public class CategoriaLivrosRepositorio : Repositorio<Categorias>, ICategoriaLivrosRepositorio
{
    #region CTOR
    public CategoriaLivrosRepositorio(AppDbContext context) : base(context) { }
    #endregion

    public async Task<IPagedList<Categorias>> GetCategoriasAsync(CategoriaParameters categoriaParameters)
    {
        var categorias = await GetAllAsync();
        var categoriasOrdenadas = categorias.OrderBy(c => c.IdCategoria).AsQueryable();
        var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriaParameters.PageNumber, categoriaParameters.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Categorias>> GetCategoriasFiltrandoPeloNome(CategoriasFiltroNome categoriasFiltroNome)
    {
        var categoria = await GetAllAsync();
        if(!string.IsNullOrEmpty(categoriasFiltroNome.Nome)) categoria = categoria.Where(c => c.NomeCategoria!.Contains(categoriasFiltroNome.Nome));

        var categoriasFiltradas = await categoria.ToPagedListAsync(categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
        return categoriasFiltradas;
    }
}
