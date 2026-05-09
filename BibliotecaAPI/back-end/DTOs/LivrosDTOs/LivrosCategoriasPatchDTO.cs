using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.LivrosDTOs;
public class LivrosCategoriasPatchDTO
{
    [Required]
    public List<int> IdsCategorias { get; set; } = new();
}
