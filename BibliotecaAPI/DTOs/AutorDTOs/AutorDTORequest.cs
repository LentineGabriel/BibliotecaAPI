using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.AutorDTOs;
public class AutorDTORequest
{
    public int IdAutor { get; set; }

    [Required]
    [StringLength(40 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? PrimeiroNome { get; set; }

    [Required]
    [StringLength(40 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? Sobrenome { get; set; }

    [Required]
    [StringLength(20 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? Nacionalidade { get; set; }

    [Required]
    public DateOnly? DataNascimento { get; set; }
}
