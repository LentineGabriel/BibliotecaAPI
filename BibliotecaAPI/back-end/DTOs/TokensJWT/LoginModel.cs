using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.DTOs.TokensJWT;

// gerenciar o login do usuário
public class LoginModel
{
    [Required(ErrorMessage = "O nome do usuário é obrigatório!")]
    public string? NomeUsuario { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória!")]
    public string? Password { get; set; }
}
