namespace BibliotecaAPI.DTOs.TokensJWT;
public class RegisterResponseDTO
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
