using BibliotecaAPI.DTOs.TokensJWT;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface IDeleteUsersUseCase
{
    Task<Response> DeleteUser(string id);
}
