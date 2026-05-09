using BibliotecaAPI.DTOs.TokensJWT;
using Microsoft.AspNetCore.Identity;

namespace BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
public interface ICreateUsersUseCase
{
    Task<LoginResponseDTO> Login(LoginModel model);
    Task<RegisterResponseDTO> Register(RegisterModel model);
    Task RefreshToken(TokenModel model);
    Task Revoke(string username);
}
