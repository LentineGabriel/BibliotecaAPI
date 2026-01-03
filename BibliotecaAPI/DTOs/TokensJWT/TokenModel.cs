namespace BibliotecaAPI.DTOs.TokensJWT;

// gerenciar o token JWT
public class TokenModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
