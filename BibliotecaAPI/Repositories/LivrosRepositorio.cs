using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.LivrosFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BibliotecaAPI.Repositories;
public class LivrosRepositorio : Repositorio<Livros>, ILivrosRepositorio
{
    #region CTOR
    public LivrosRepositorio(AppDbContext context) : base(context) { }
    #endregion

    #region METHODS
    public async Task<IEnumerable<Livros?>> GetLivroCompletoAsync()
    {
        return await _context.Livros
            .Include(l => l.Autor)
            .Include(l => l.Editora)
            .Include(l => l.LivrosCategorias!).ThenInclude(lc => lc.Categorias)
            .ToListAsync();
    }

    public async Task<Livros?> GetLivroCompletoAsync(int id)
    {
        return await _context.Livros
            .Include(l => l.Autor)
            .Include(l => l.Editora)
            .Include(l => l.LivrosCategorias!).ThenInclude(lc => lc.Categorias)
            .FirstOrDefaultAsync(l => l.IdLivro == id);
    }

    public async Task<IPagedList<Livros?>> GetLivrosAsync(LivrosParameters livrosParameters)
    {
        var livros = await GetLivroCompletoAsync();
        var livrosOrdenados = livros.OrderBy(l => l?.IdLivro).AsQueryable();
        var resultado = await livrosOrdenados.ToPagedListAsync(livrosParameters.PageNumber , livrosParameters.PageSize);

        return resultado;
    }

    public async Task<IPagedList<Livros?>> GetLivrosFiltrandoPeloNome(LivrosFiltroNome livrosFiltroNome)
    {
        var livros = await GetLivroCompletoAsync();
        if(!string.IsNullOrEmpty(livrosFiltroNome.Nome))
        {
            var termo = livrosFiltroNome.Nome.ToLower();
            livros = livros.Where(l => l?.NomeLivro != null && l.NomeLivro.ToLower().Contains(termo)).ToList();
        }

        var livrosFiltrados = await livros.ToPagedListAsync(livrosFiltroNome.PageNumber , livrosFiltroNome.PageSize);
        return livrosFiltrados;
    }

    public async Task<IPagedList<Livros?>> GetLivrosFiltrandoPeloAutor(LivrosFiltroAutor livrosFiltroAutor)
    {
        var livros = await GetLivroCompletoAsync();
        if(!string.IsNullOrWhiteSpace(livrosFiltroAutor.Autor))
        {
            var filtro = livrosFiltroAutor.Autor.Trim().ToLower();
            livros = livros.Where(l => l?.Autor != null && l.Autor.NomeCompleto!.ToLower().Contains(filtro)).ToList();
        }

        var livrosFiltrados = await livros.ToPagedListAsync(livrosFiltroAutor.PageNumber, livrosFiltroAutor.PageSize);
        return livrosFiltrados;
    }

    public async Task<IPagedList<Livros?>> GetLivrosFiltrandoPelaEditora(LivrosFiltroEditora livrosFiltroEditora)
    {
        var livros = await GetLivroCompletoAsync();
        if(!string.IsNullOrWhiteSpace(livrosFiltroEditora.Editora))
        {
            var filtro = livrosFiltroEditora.Editora.Trim().ToLower();
            livros = livros.Where(l => l?.Editora != null && l.Editora.NomeEditora!.ToLower().Contains(filtro)).ToList();
        }

        var livrosFiltrados = await livros.ToPagedListAsync(livrosFiltroEditora.PageNumber, livrosFiltroEditora.PageSize);
        return livrosFiltrados;
    }

    public async Task<IPagedList<Livros?>> GetLivrosFiltrandoPorAnoPublicacao(LivrosFiltroAnoPublicacao livrosFiltroAnoPublicacao)
    {
        var livros = await GetLivroCompletoAsync();
        livros = livros.Where(l => l?.AnoPublicacao == livrosFiltroAnoPublicacao.AnoPublicacao).ToList();

        var livrosFiltrados = await livros.ToPagedListAsync(livrosFiltroAnoPublicacao.PageNumber, livrosFiltroAnoPublicacao.PageSize);
        return livrosFiltrados;
    }

    public async Task<IPagedList<Livros?>> GetLivrosFiltrandoPorCategoria(LivrosFiltroCategoria livrosFiltroCategoria)
    {
        var livros = await GetLivroCompletoAsync();
        if(!string.IsNullOrWhiteSpace(livrosFiltroCategoria.Categorias))
        {
            var filtro = livrosFiltroCategoria.Categorias.Trim().ToLower();
            livros = livros.Where(l => l?.LivrosCategorias != null && l.LivrosCategorias.Any(lc => lc.Categorias.NomeCategoria!.ToLower().Contains(filtro)));
        }

        var livrosFiltrados = await livros.ToPagedListAsync(livrosFiltroCategoria.PageNumber, livrosFiltroCategoria.PageSize);
        return livrosFiltrados;
    }
    #endregion
}
