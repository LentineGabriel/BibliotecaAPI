using BibliotecaAPI.DTOs.TokensJWT;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface ICreateUsersUseCase
{
    Task<IdentityResult> Login(LoginModel model);
    Task Register(RegisterModel model);
    Task RefreshToken(TokenModel model);
    Task Revoke(string username);
}
