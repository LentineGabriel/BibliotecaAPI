using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
public interface ICreateRolesUseCase
{
    Task<IActionResult> CreateRole(string roleName);
    Task<IActionResult> AddUserToRole(string email , string roleName);
}
