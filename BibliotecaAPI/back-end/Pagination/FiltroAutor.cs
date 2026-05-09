namespace BibliotecaAPI.Pagination;

public class FiltroAutor<T> : QueryStringParameters where T : class
{
    public string? Autor { get; set; }
}
