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

    // Chave estrangeira - Autor
    public int IdAutor { get; set; }

    [JsonIgnore]
    [ForeignKey("IdAutor")]
    public Autor? Autor { get; set; }

    // Chave estrangeira - Editora
    public int IdEditora { get; set; }

    [JsonIgnore]
    [ForeignKey("IdEditora")]
    public Editoras? Editora { get; set; }

    [Required]
    [Range(1500 , 2100 , ErrorMessage = "O ano de publicação deve estar entre {1} e {2}")]
    public int AnoPublicacao { get; set; }

    [JsonIgnore]
    public ICollection<LivroCategoria>? LivrosCategorias { get; set; }

    public Livros()
    {
        LivrosCategorias = new List<LivroCategoria>();
    }
    #endregion

    #region METHODS
    // Algumas validações personalizadas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var campos = new Dictionary<string , string?>
        {
            { nameof(NomeLivro), NomeLivro }
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
