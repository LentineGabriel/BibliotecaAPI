namespace BibliotecaAPI.Pagination;
public class FiltroNacionalidade<T> : QueryStringParameters where T : class
{
    public string? Nacionalidade { get; set; }
}
