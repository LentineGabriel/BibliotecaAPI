namespace BibliotecaAPI.DTOs.AutorDTOs;
public class AutorDTOResponse
{
    public int IdAutor { get; set; }
    public string? PrimeiroNome { get; set; }
    public string? Sobrenome { get; set; }
    public string? Nacionalidade { get; set; }
    public DateOnly? DataNascimento { get; set; }
}
