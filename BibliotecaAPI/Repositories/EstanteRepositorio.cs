using BibliotecaAPI.Context;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class EstanteRepositorio : IEstanteRepositorio
{
    #region PROPS/CTOR
    private readonly AppDbContext _context;

    public EstanteRepositorio(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    public async Task AddAsync(Estante estante) => 
        await _context.Estantes.AddAsync(estante);
    
    public async Task<Estante?> GetByIdAsync(int id) =>
        await _context.Estantes.Include(e => e.Livro!).FirstOrDefaultAsync(e => e.IdEstante == id);
    
    public async Task<Estante?> GetByUserAndLivroAsync(string userId , int livroId) =>
        await _context.Estantes.FirstOrDefaultAsync(e => e.UserId == userId && e.LivroId == livroId);

    public async Task<IEnumerable<Estante>> GetEstanteUsuarioAsync(string userId , int page , int pageSize) =>
        await _context.Estantes.Where(e => e.UserId == userId)
                                .Include(e => e.Livro!)
                                    .ThenInclude(l => l!.Autor)
                                .Include(e => e.Livro!)
                                    .ThenInclude(l => l!.Editora)
                                .Include(e => e.Livro!)
                                    .ThenInclude(l => l!.LivrosCategorias)
                                        .ThenInclude(lc => lc.Categorias)
                                .Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Remove(Estante estante) => _context.Remove(estante);
}
