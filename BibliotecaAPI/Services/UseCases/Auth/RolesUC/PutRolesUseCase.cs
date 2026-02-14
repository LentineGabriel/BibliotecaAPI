using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Roles;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services.UseCases.Auth.RolesUC;

public class PutRolesUseCase : IPutRolesUseCase
{
    #region PROPS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public PutRolesUseCase(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<RolesResponseDTO> PutRoleAsync(string id , RolesRequestDTO rolesDTO)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role == null) throw new NullReferenceException($"Não foi possível encontrar o perfil com o nome '{id}'. Por favor, verifique o nome digitado e tente novamente!");

        role.Name = rolesDTO.Name;

        var result = await _roleManager.UpdateAsync(role);
        if(!result.Succeeded)
        {
            var errors = result.Errors?
                .Select(e => string.IsNullOrWhiteSpace(e.Description) ? e.Code : $"{e.Code}: {e.Description}")
                .ToArray() ?? Array.Empty<string>();

            var message = errors.Length > 0
                ? $"Falha ao atualizar o perfil: {string.Join("; ", errors)}"
                : "Falha ao atualizar o perfil por motivo desconhecido.";

            throw new InvalidOperationException(message);
        }

        return _mapper.Map<RolesResponseDTO>(role);
    }
}
