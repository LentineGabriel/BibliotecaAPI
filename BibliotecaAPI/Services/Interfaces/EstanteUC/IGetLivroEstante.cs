namespace BibliotecaAPI.Services.Interfaces.EstanteUC;
public interface IGetLivroEstante
{
    Task<IEnumerable<Estante>> GetAsync(string userId, int page, int pageSize);
}
