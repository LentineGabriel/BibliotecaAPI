namespace BibliotecaAPI.DTOs.AuthDTOs.Users;
public class UsersRequestDTO
{
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
}
