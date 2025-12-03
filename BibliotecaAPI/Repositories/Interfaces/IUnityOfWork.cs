namespace BibliotecaAPI.Repositories.Interfaces;
public interface IUnityOfWork
{
    ILivrosRepositorio LivrosRepositorio { get; }
    IAutorRepositorio AutorRepositorio { get; }
    IEditorasRepositorio EditorasRepositorio { get; }
    ICategoriaLivrosRepositorio CategoriaLivrosRepositorio { get; }
    Task CommitAsync();
    Task DisposeAsync();
}
