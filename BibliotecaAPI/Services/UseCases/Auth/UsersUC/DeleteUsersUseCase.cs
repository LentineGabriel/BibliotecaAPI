using AutoMapper;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using Microsoft.AspNetCore.Identity;

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

    public async Task<Response> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new Response
            {
                Status = "Erro",
                Message = $"Usuário com id '{id}' não encontrado."
            };
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return new Response
            {
                Status = "Sucesso",
                Message = $"Usuário '{user.UserName}' deletado com sucesso."
            };
        }

        var errors = result.Errors != null && result.Errors.Any() ? string.Join("; ", result.Errors.Select(e => e.Description ?? e.Code ?? e.ToString())) : "Erro desconhecido ao deletar o usuário.";

        return new Response
        {
            Status = "Erro",
            Message = $"Erro ao deletar o usuário '{user.UserName}': {errors}"
        };
    }
}
