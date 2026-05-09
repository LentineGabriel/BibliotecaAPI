using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Pagination.UsuariosFiltro;
using X.PagedList;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IGetUsersUseCase
{
    Task<IEnumerable<UsersDTOResponse>> GetUsersAsync();
    Task<UsersDTOResponse> GetUserByIdAsync(string id);
    Task<IPagedList<UsersDTOResponse>> GetPaginationAsync(UsuariosParameters usuariosParameters);
    Task<IPagedList<UsersDTOResponse>> GetUserEstanteAsync(string id);
}