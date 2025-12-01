namespace BibliotecaAPI.Repositories.Interfaces;
public interface IUnityOfWork
{
    ILivrosRepositorio LivrosRepositorio { get; }
    Task CommitAsync();
    Task DisposeAsync();
}
