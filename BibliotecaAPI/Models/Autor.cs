using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;
public class Autor : IValidatableObject
{
    #region PROPS/CTOR
    [Key]
    [Required]
    public int IdAutor { get; set; }

    [Required]
    [StringLength(40 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? PrimeiroNome { get; set; }

    [Required]
    [StringLength(40 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? Sobrenome { get; set; }

    public string? NomeCompleto { get => PrimeiroNome + " " + Sobrenome; }

    [Required]
    [StringLength(20 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? Nacionalidade { get; set; }

    [Required]
    public DateOnly? DataNascimento { get; set; }

    public ICollection<Livros>? Livros { get; set; }
    #endregion

    #region METHODS
    // Algumas validações personalizadas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var campos = new Dictionary<string , string?>
        {
            { nameof(PrimeiroNome), PrimeiroNome },
            { nameof(Sobrenome), Sobrenome },
            { nameof(Nacionalidade), Nacionalidade }
        };

        foreach(var c in campos)
        {
            var valor = c.Value;

            if(!string.IsNullOrWhiteSpace(valor))
            {
                var primeira = valor[0].ToString();
                if(primeira != primeira.ToUpper()) yield return new ValidationResult($"A primeira letra de {c.Key} deve ser maiúscula" , new[] { c.Key });
            }
        }

        if(DataNascimento.HasValue)
        {
            var ano = DataNascimento.Value.Year;
            if(ano < 1500 || ano > 3000) yield return new ValidationResult("O ano de nascimento deve estar entre 1500 e 3000" , new[] { nameof(DataNascimento) });
        }
    }
    #endregion
}
