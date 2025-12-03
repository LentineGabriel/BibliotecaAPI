using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaAPI.Models;

[Table("Editoras")]
public class Editoras : IValidatableObject
{
    #region PROPS/CTOR
    [Key]
    [Required]
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

    [Url(ErrorMessage = "Informe uma URL válida")]
    public string? SiteOficial { get; set; }

    public ICollection<Livros>? Livros { get; set; }
    #endregion

    #region METHODS
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var campos = new Dictionary<string , string?>
        {
            { nameof(NomeEditora), NomeEditora },
            { nameof(PaisOrigem), PaisOrigem }
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
    }
    #endregion
}
