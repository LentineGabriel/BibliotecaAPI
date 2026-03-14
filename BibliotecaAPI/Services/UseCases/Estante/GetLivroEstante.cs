using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EstanteUC;

namespace BibliotecaAPI.Services.UseCases.EstanteUC;

public class GetLivroEstante : IGetLivroEstante
{
    #region PROPS/CTOR
    private readonly IEstanteRepositorio _estanteRepositorio;

    public GetLivroEstante(IEstanteRepositorio estanteRepositorio)
    {
        _estanteRepositorio = estanteRepositorio;
    }
    #endregion

    public async Task<IEnumerable<Estante>> GetAsync(string userId , int page , int pageSize) => await _estanteRepositorio.GetEstanteUsuarioAsync(userId , page , pageSize);
}
