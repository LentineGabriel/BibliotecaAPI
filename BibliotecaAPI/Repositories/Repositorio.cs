using BibliotecaAPI.Context;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BibliotecaAPI.Repositories;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    #region PROPS/CTOR
    protected readonly AppDbContext _context;

    public Repositorio(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region METHODS
    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().AsNoTracking().ToListAsync();

    public async Task<T?> GetIdAsync(Expression<Func<T , bool>> predicate) => await _context.Set<T>().FirstOrDefaultAsync(predicate);

    public T Create(T item)
    {
        _context.Set<T>().Add(item);
        return item;
    }

    public T Update(T item)
    {
        _context.Set<T>().Update(item);
        return item;
    }

    public T Delete(T item)
    {
        _context.Set<T>().Remove(item);
        return item;
    }
    #endregion
}
