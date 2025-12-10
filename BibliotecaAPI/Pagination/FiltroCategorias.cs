using BibliotecaAPI.Models;

namespace BibliotecaAPI.Pagination;
public class FiltroCategorias<T> : QueryStringParameters where T : class
{
    public string? Categorias { get; set; }
}
