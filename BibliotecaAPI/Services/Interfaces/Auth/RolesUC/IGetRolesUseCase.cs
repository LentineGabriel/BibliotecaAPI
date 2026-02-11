using BibliotecaAPI.DTOs.AuthDTOs.Roles;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Pagination.PerfilFiltro;
using Microsoft.AspNetCore.Identity;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
public interface IGetRolesUseCase
{
    Task<IEnumerable<RolesResponseDTO>> GetRolesAsync();
    Task<RolesResponseDTO> GetRoleByIdAsync(string id);
    Task<IEnumerable<UsersResponseDTO>> GetUsersInRoleAsync(string perfil);
    Task<IPagedList<RolesResponseDTO>> GetPaginationAsync(PerfilParameters perfilParameters);
}
