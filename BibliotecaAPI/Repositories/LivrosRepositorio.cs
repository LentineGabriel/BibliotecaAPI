using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;
public class LivrosRepositorio : Repositorio<Livros>, ILivrosRepositorio
{
    public LivrosRepositorio(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Livros>> GetLivrosComAutorAsync() => await _context.Livros.Include(l => l.Autor).ToListAsync();

    public async Task<Livros?> GetLivroComAutorAsync(int id) => await _context.Livros.Include(l => l.Autor).FirstOrDefaultAsync(l => l.IdLivro == id);
}
