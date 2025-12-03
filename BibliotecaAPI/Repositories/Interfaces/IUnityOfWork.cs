namespace BibliotecaAPI.Repositories.Interfaces;
public interface IUnityOfWork
{
    ILivrosRepositorio LivrosRepositorio { get; }
    IAutorRepositorio AutorRepositorio { get; }
    ICategoriaLivrosRepositorio CategoriaLivrosRepositorio { get; }
    Task CommitAsync();
    Task DisposeAsync();
}
