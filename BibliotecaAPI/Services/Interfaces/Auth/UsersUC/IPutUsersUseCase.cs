using BibliotecaAPI.DTOs.AuthDTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IPutUsersUseCase
{
    Task<UsersResponseDTO> PutAsync(string id , UsersRequestDTO usersDTO);
}
