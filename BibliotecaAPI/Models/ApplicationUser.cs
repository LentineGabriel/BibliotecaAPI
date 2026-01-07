using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Models;
public class ApplicationUser : IdentityUser
{
    // registrando o Refresh Token e seu tempo de expiração
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
