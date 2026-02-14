using AutoMapper;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.UseCases.Auth.RolesUC;
public class DeleteRolesUseCase : IDeleteRolesUseCase
{
    #region PROPS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public DeleteRolesUseCase(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role != null)
        {
            var result = await _roleManager.DeleteAsync(role);
            if(result.Succeeded)
            {
                var response = new Response
                {
                    Status = "Sucesso",
                    Message = $"Perfil '{role.Name}' deletado com sucesso."
                };
                return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                var response = new Response
                {
                    Status = "Erro",
                    Message = $"Erro ao deletar o perfil '{role.Name}'."
                };
                return new BadRequestObjectResult(response);
            }
        }

        return new BadRequestObjectResult(new { Error = "Não foi possível encontrar o perfil. Por favor, tente novamente!" });
    }
}
