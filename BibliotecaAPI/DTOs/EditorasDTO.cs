using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;
public class EditorasDTO
{
    public int IdEditora { get; set; }

    [Required]
    [StringLength(80 , MinimumLength = 4 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeEditora { get; set; }

    [Required]
    [StringLength(15 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? PaisOrigem { get; set; }

    [Required]
    [Range(1500 , 2030 , ErrorMessage = "O ano de fundação deve estar entre {1} e {2}")]
    public int AnoFundacao { get; set; }

    public string? SiteOficial { get; set; }
}
