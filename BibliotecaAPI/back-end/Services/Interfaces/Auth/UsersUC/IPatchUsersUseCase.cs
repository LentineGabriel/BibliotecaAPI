using BibliotecaAPI.DTOs.AuthDTOs.Users;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IPatchUsersUseCase
{
    Task<UsersDTOResponse> PatchAsync(string id , JsonPatchDocument<UsersDTOResponse> patchDoc);
}