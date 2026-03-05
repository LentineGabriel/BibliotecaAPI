using BibliotecaAPI.DTOs.TokensJWT;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IDeleteUsersUseCase
{
    Task<IActionResult> DeleteUser(string id);
}
