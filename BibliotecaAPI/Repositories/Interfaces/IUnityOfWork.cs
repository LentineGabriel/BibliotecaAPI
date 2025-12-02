namespace BibliotecaAPI.Repositories.Interfaces;
public interface IUnityOfWork
{
    ILivrosRepositorio LivrosRepositorio { get; }
    ICategoriaLivrosRepositorio CategoriaLivrosRepositorio { get; }
    Task CommitAsync();
    Task DisposeAsync();
}
