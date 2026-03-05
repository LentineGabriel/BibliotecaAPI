using AutoMapper;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.UseCases.Auth.UsersUC;

public class DeleteUsersUseCase : IDeleteUsersUseCase
{
    #region PROS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public DeleteUsersUseCase(UserManager<ApplicationUser> userManager , IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                var response = new Response
                {
                    Status = "Sucesso" ,
                    Message = $"Usuário '{user.UserName}' deletado com sucesso."
                };
                return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                var response = new Response
                {
                    Status = "Erro" ,
                    Message = $"Erro ao deletar o Usuário '{user.UserName}'."
                };
                return new BadRequestObjectResult(response);
            }
        }

        return new BadRequestObjectResult(new { Error = "Não foi possível encontrar o Usuário. Por favor, tente novamente!" });
    }
}
