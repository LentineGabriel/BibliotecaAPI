namespace BibliotecaAPI.DTOs.LivrosDTOs;
public class LivrosDTOResponse
{
    public int IdLivro { get; set; }
    public string? NomeLivro { get; set; }
    public string? NomeAutor { get; set; }
    public string? NomeEditora { get; set; }
    public int AnoPublicacao { get; set; }
    public List<string>? Categorias { get; set; } = new();
}
