using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Roles;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.PerfilFiltro;
using BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.Auth.RolesUC;

public class GetRolesUseCase : IGetRolesUseCase
{
    #region PROPS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public GetRolesUseCase(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<RolesResponseDTO>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return _mapper.Map<IEnumerable<RolesResponseDTO>>(roles);
    }

    public async Task<RolesResponseDTO> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role == null) throw new KeyNotFoundException($"Não foi possível encontrar o perfil com o ID {id}. Por favor, tente novamente!");

        return _mapper.Map<RolesResponseDTO>(role);
    }

    public async Task<IEnumerable<UsersResponseDTO>> GetUsersInRoleAsync(string perfil)
    {
        var usuariosNoPerfil = await _userManager.GetUsersInRoleAsync(perfil);
        if(usuariosNoPerfil == null || !usuariosNoPerfil.Any()) throw new NullReferenceException($"Nenhum usuário encontrado no perfil '{perfil}'. Por favor, tente novamente!");

        return _mapper.Map<IEnumerable<UsersResponseDTO>>(usuariosNoPerfil);
    }

    public async Task<IPagedList<RolesResponseDTO>> GetPaginationAsync(PerfilParameters perfilParameters) => (IPagedList<RolesResponseDTO>)await _roleManager.Roles.ToListAsync();
}
