using BibliotecaAPI.Enums;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.DTOs.EstanteDTOs;
public class EstanteDTOResponse
{
    public string? NomeLivro { get; set; }
    public string? NomeAutor { get; set; }
    public string? NomeEditora { get; set; }
    public int AnoPublicacao { get; set; }
    public StatusLivroEstante StatusLeitura { get; set; } = StatusLivroEstante.QueroLer;
    public List<string>? Categorias { get; set; } = new();
}
