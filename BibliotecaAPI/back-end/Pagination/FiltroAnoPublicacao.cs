namespace BibliotecaAPI.Pagination;
public class FiltroAnoPublicacao<T> : QueryStringParameters where T : class
{
    public int AnoPublicacao { get; set; }
}
