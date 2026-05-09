using BibliotecaAPI.DTOs.AuthDTOs.Roles;

namespace BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
public interface IPutRolesUseCase
{
    Task<RolesResponseDTO> PutRoleAsync(string id , RolesRequestDTO rolesDTO);
}
