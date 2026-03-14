

namespace BibliotecaAPI.Services.Interfaces.EstanteUC;
public interface ICreateLivroEstante
{
    Task<Estante> CreateAsync(string userId, int livroId);
}
