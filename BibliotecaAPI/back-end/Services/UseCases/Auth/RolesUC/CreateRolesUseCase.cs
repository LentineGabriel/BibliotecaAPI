using AutoMapper;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.UseCases.Auth.RolesUC;

public class CreateRolesUseCase : ICreateRolesUseCase
{
    #region PROPS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public CreateRolesUseCase(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (roleResult != null && roleResult.Succeeded)
            {
                return new ObjectResult(new Response { Status = "Sucesso", Message = $"Role '{roleName}' adicionada com sucesso!" })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                return new ObjectResult(new Response { Status = "Erro", Message = $"Erro ao adicionar a role '{roleName}'." })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }

        return new ObjectResult(new Response { Status = "Erro", Message = $"Role '{roleName}' já existe." })
        {
            StatusCode = StatusCodes.Status409Conflict
        };
    }

    public async Task<IActionResult> AddUserToRole(string email , string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new ObjectResult(new Response
            {
                Status = "Erro",
                Message = $"Usuário com email '{email}' não encontrado."
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            return new ObjectResult(new Response
            {
                Status = "Sucesso",
                Message = $"Usuário '{user.Email}' adicionado ao perfil '{roleName}'."
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        else
        {
            return new ObjectResult(new Response
            {
                Status = "Erro",
                Message = $"Erro ao adicionar o usuário '{user.Email}' ao perfil '{roleName}'."
            })
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
