namespace BibliotecaAPI.DTOs.AuthDTOs.Users;
public class UsersResponseDTO
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
}
