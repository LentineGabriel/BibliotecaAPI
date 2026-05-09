using BibliotecaAPI.Enums;
using BibliotecaAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.DTOs.EstanteDTOs;
public class EstanteDTORequest
{
    [Key]
    public int IdEstante { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int LivroId { get; set; }

    [Required]
    public StatusLivroEstante StatusLeitura { get; set; } = StatusLivroEstante.QueroLer;

    [ForeignKey(nameof(LivroId))]
    public Livros? Livro { get; set; }
}
