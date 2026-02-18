using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Pagination.UsuariosFiltro;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IGetUsersUseCase
{
    Task<IEnumerable<UsersResponseDTO>> GetUsersAsync();
    Task<UsersResponseDTO> GetUserByIdAsync(string id);
    Task<IPagedList<UsersResponseDTO>> GetPaginationAsync(UsuariosParameters usuariosParameters);
}