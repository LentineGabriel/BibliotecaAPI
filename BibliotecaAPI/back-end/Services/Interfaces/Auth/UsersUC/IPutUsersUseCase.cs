using BibliotecaAPI.DTOs.AuthDTOs.Users;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IPutUsersUseCase
{
    Task<UsersDTOResponse> PutAsync(string id , UsersDTORequest usersDTO);
}
