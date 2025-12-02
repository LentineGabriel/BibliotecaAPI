using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs;
public class CategoriasDTO
{
    public int IdCategoria { get; set; }

    [Required]
    [StringLength(100 , MinimumLength = 4 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeCategoria { get; set; }

    [Required]
    [StringLength(80 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? DescricaoCategoria { get; set; }
}
