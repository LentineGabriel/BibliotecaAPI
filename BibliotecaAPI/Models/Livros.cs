#region USINGS
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
#endregion

namespace BibliotecaAPI.Models;

[Table("Livros")]
public class Livros : IValidatableObject
{
    #region PROPS/CTOR
    [Key]
    [Required]
    public int IdLivro { get; set; }

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

    // Chave estrangeira
    [ForeignKey("CategoriaLivro")]
    public int IdCategoria { get; set; }

    [JsonIgnore]
    public Categorias? CategoriaLivro { get; set; }
    #endregion

    #region METHODS
    // Algumas validações personalizadas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var campos = new Dictionary<string , string?>
        {
            { nameof(NomeLivro), NomeLivro },
            { nameof(NomeAutor), NomeAutor },
            { nameof(NomeEditora), NomeEditora }
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
