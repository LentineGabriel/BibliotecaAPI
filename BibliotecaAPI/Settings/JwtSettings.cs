namespace BibliotecaAPI.Settings;
public class JwtSettings
{
    // Props utilizadas p/ lidar com o token JWT
    public string ValidAudience { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int TokenValidityInMinutes { get; set; }
    public int RefreshTokenValidityInMinutes { get; set; }
}
