using System.Linq.Expressions;

namespace BibliotecaAPI.Repositories.Interfaces;
public interface IRepositorio<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetIdAsync(Expression<Func<T, bool>> predicate);
    T Create(T item);
    T Update(T item);
    T Delete(T item);
}
