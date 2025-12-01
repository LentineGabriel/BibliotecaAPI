using BibliotecaAPI.Context;
using BibliotecaAPI.Repositories.Interfaces;

namespace BibliotecaAPI.Repositories;

public class UnityOfWork : IUnityOfWork
{
    #region PROPS/CTOR
    private ILivrosRepositorio? _livrosRepositorio;
    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region METHODS
    public ILivrosRepositorio LivrosRepositorio => _livrosRepositorio = _livrosRepositorio ?? new LivrosRepositorio(_context);
    public async Task CommitAsync() => await _context.SaveChangesAsync();
    public async Task DisposeAsync() => await _context.DisposeAsync();
    #endregion
}
