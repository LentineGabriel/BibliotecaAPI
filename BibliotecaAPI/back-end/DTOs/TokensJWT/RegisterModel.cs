using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.TokensJWT;

// gerenciar o registro do usuário
public class RegisterModel
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório!")]
    public string? NomeUsuario { get; set; }

    [Required(ErrorMessage = "O email do usuário é obrigatório!")]
    [EmailAddress(ErrorMessage = "O email digitado está inválido! Verifique-o e digite novamente")]
    public string? EmailUsuario { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória!")]
    public string? Password { get; set; }
}
