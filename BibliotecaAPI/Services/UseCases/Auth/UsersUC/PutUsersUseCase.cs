using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.TokenJWT;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services.UseCases.Auth.UsersUC;

public class PutUsersUseCase : IPutUsersUseCase
{
    #region PROS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public PutUsersUseCase(UserManager<ApplicationUser> userManager , IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<UsersDTOResponse> PutAsync(string id , UsersDTORequest usersDTO)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user == null) throw new ArgumentException($"Não foi possível encontrar o usuário com ID {id}. Por favor, verifique o id digitado e tente novamente!", nameof(id));

        user.UserName = usersDTO.Username;
        user.Email = usersDTO.Email;
        user.EmailConfirmed = usersDTO.EmailConfirmed;

        var result = await _userManager.UpdateAsync(user);
        if(!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return _mapper.Map<UsersDTOResponse>(user);
    }
}
