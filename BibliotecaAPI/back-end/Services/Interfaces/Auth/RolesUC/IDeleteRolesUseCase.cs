using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
public interface IDeleteRolesUseCase
{
    Task<IActionResult> DeleteRole(string id);
}
