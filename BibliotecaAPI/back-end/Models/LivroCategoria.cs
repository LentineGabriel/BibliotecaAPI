namespace BibliotecaAPI.Models;

public class LivroCategoria
{
    public int LivroId { get; set; }
    public Livros Livros { get; set; } = null!;
    public int CategoriaId { get; set; }
    public Categorias Categorias { get; set; } = null!;
}