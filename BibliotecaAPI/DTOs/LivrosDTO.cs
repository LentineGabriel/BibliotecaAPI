using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;
public class LivrosDTO
{
    [Required]
    [StringLength(150 , MinimumLength = 4 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeLivro { get; set; }

    [Required]
    [StringLength(100 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeAutor { get; set; }

    [Required]
    [StringLength(80 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeEditora { get; set; }

    [Required]
    [Range(1500 , 2100 , ErrorMessage = "O ano de publicação deve estar entre {1} e {2}")]
    public int AnoPublicacao { get; set; }
}
