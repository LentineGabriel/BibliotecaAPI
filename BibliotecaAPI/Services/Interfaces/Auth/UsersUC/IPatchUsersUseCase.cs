using BibliotecaAPI.DTOs.AuthDTOs.Users;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IPatchUsersUseCase
{
    Task<UsersResponseDTO> PatchAsync(string id , JsonPatchDocument<UsersResponseDTO> patchDoc);
}