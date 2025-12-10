using BibliotecaAPI.Models;

namespace BibliotecaAPI.Pagination;
public class FiltroEditora<T> : QueryStringParameters where T : class
{
    public string? Editora { get; set; }
}
