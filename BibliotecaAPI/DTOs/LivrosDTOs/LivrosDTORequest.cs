using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.LivrosDTOs;
public class LivrosDTORequest
{
    public int IdLivro { get; set; }

    [Required]
    [StringLength(150 , MinimumLength = 4 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeLivro { get; set; }

    [Required]
    public int IdAutor { get; set; }
    public string? NomeAutor { get; set; }

    [Required]
    public int IdEditora { get; set; }
    public string? NomeEditora { get; set; }

    [Required]
    [Range(1500 , 2100 , ErrorMessage = "O ano de publicação deve estar entre {1} e {2}")]
    public int AnoPublicacao { get; set; }

    [Required]
    public int IdCategoria { get; set; }
    public string? NomeCategoria { get; set; }
}
