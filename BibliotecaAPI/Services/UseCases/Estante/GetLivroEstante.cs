using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EstanteUC;

namespace BibliotecaAPI.Services.UseCases.EstanteUC;

public class GetLivroEstante : IGetLivroEstante
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;

    public GetLivroEstante(IUnityOfWork uof)
    {
        _uof = uof;
    }
    #endregion

    public async Task<IEnumerable<Estante>> GetAsync(string userId , int page , int pageSize) => await _uof.EstanteRepositorio.GetEstanteUsuarioAsync(userId , page , pageSize);
    public async Task<IEnumerable<Estante>> SearchBooksAsync(string userId , string termo) => await _uof.EstanteRepositorio.SearchBooksAsync(termo);
}
