using BibliotecaAPI.Enums;
using BibliotecaAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Estante : IValidatableObject
{
    #region PROPS/CTOR
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

    #endregion

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(LivroId <= 0)
        {
            yield return new ValidationResult(
                "LivroId inválido" ,
                new[] { nameof(LivroId) }
            );
        }

        if(string.IsNullOrWhiteSpace(UserId))
        {
            yield return new ValidationResult(
                "UserId é obrigatório" ,
                new[] { nameof(UserId) }
            );
        }
    }
}
