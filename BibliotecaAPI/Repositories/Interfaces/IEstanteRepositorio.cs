namespace BibliotecaAPI.Repositories.Interfaces;
public interface IEstanteRepositorio
{
    Task AddAsync(Estante estante);
    Task<Estante?> GetByIdAsync(int id); // atualizar status
    Task<Estante?> GetByUserAndLivroAsync(string userId , int livroId); // evitar duplicidade
    Task<IEnumerable<Estante>> GetEstanteUsuarioAsync(string userId , int page , int pageSize); // paginação
    Task<IEnumerable<Estante>> SearchBooksAsync(string userId , int page , int pageSize , string termo); // busca por título
    Task SaveChangesAsync();
    void Remove(Estante estante);
}
