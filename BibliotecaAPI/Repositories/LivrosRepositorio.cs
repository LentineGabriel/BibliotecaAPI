using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;
public class LivrosRepositorio : Repositorio<Livros>, ILivrosRepositorio
{
    public LivrosRepositorio(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Livros?>> GetLivroCompletoAsync()
    {
        return await _context.Livros
            .Include(l => l.Autor)
            .Include(l => l.Editora)
            .Include(l => l.CategoriaLivro)
            .ToListAsync();
    }

    public async Task<Livros?> GetLivroCompletoAsync(int id)
    {
        return await _context.Livros
            .Include(l => l.Autor)
            .Include(l => l.Editora)
            .Include(l => l.CategoriaLivro)
            .FirstOrDefaultAsync(l => l.IdLivro == id);
    }
}
