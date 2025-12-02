using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Models;

public class Categorias : IValidatableObject
{
    // essas categorias serão os gêneros dos livros
    #region PROPS/CTOR
    [Key]
    [Required]
    public int IdCategoria { get; set; }

    [Required]
    [StringLength(100 , MinimumLength = 4 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? NomeCategoria { get; set; }

    [Required]
    [StringLength(80 , MinimumLength = 2 , ErrorMessage = "O valor permitido de caracteres deve estar entre {2} e {1}")]
    public string? DescricaoCategoria { get; set; }

    public ICollection<Livros>? Livros { get; set; }

    public Categorias()
    {
        Livros = new Collection<Livros>();
    }
    #endregion

    #region METHODS
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var campos = new Dictionary<string , string?>
        {
            { nameof(NomeCategoria), NomeCategoria },
            { nameof(DescricaoCategoria), DescricaoCategoria }
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
