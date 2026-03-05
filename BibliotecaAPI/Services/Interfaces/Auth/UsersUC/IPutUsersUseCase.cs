using BibliotecaAPI.DTOs.AuthDTOs.Users;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IPutUsersUseCase
{
    Task<UsersResponseDTO> PutAsync(string id , UsersRequestDTO usersDTO);
}
