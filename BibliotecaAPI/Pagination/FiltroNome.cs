namespace BibliotecaAPI.Pagination;
public class FiltroNome<T> : QueryStringParameters where T : class
{
    public string? Nome { get; set; }
}
