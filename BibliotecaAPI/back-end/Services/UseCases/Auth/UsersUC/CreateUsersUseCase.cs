using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.TokenJWT;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BibliotecaAPI.Services.UseCases.Auth.UsersUC;

public class CreateUsersUseCase : ICreateUsersUseCase
{
    #region PROS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _cfg;

    public CreateUsersUseCase(UserManager<ApplicationUser> userManager , ITokenService tokenService , IConfiguration cfg)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _cfg = cfg;
    }
    #endregion

    public async Task<LoginResponseDTO> Login(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.NomeUsuario!);
        if (user is not null && await _userManager.CheckPasswordAsync(user , model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // guid id
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), // id usuário
                new Claim(ClaimTypes.Name, user.UserName!), // nome usuário
                new Claim(ClaimTypes.Email, user.Email!), // email usuário
            };

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role , userRole));
            }

            // Generate JWT (may return JwtSecurityToken) and convert to string
            var jwtToken = _tokenService.GenerateAccessToken(authClaims);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = _tokenService.GenerateRefreshToken();

            // Salvar refresh token no banco de dados
            if(!int.TryParse(_cfg["JWT:RefreshTokenValidityInMinutes"] , out int refreshTokenValidityInMinutes) || refreshTokenValidityInMinutes <= 0)
                throw new InvalidOperationException("Configuração inválida para validade do refresh token.");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

            await _userManager.UpdateAsync(user);
            return new LoginResponseDTO
            {
                AccessToken = token ,
                RefreshToken = refreshToken ,
                Expiration = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes)
            };
        }

        throw new UnauthorizedAccessException("Credenciais inválidas");
    }

    public async Task<RegisterResponseDTO> Register(RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.NomeUsuario ,
            Email = model.EmailUsuario
        };

        var result = await _userManager.CreateAsync(user , model.Password!);

        if(!result.Succeeded) throw new ApplicationException(string.Join(" | " , result.Errors.Select(e => e.Description)));

        // Gerar claims
        var authClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        // Generate JWT token (JwtSecurityToken) and convert to string before assigning to DTO
        var jwtToken = _tokenService.GenerateAccessToken(authClaims);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["JWT:RefreshTokenValidityInMinutes"]!));

        await _userManager.UpdateAsync(user);

        return new RegisterResponseDTO
        {
            AccessToken = token ,
            RefreshToken = refreshToken ,
            Expiration = user.RefreshTokenExpiryTime!.Value
        };
    }

    public async Task RefreshToken(TokenModel model)
    {
        var accessToken = model.AccessToken;
        var refreshToken = model.RefreshToken;
        if(string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) throw new ArgumentException("Token inválido.");

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        if(principal == null) throw new UnauthorizedAccessException("Token inválido.");

        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if(string.IsNullOrWhiteSpace(userId)) throw new UnauthorizedAccessException("Token inválido.");

        var user = await _userManager.FindByIdAsync(userId);
        if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) throw new UnauthorizedAccessException("Token inválido ou expirado.");

        // Convert newly generated token to string to match expected usage
        var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
        var newAccessToken = new JwtSecurityTokenHandler().WriteToken(newJwtToken);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = _tokenService.GetRefreshTokenExpiry();
        await _userManager.UpdateAsync(user);

        return;
    }

    public async Task Revoke(string username)
    {
        if(string.IsNullOrWhiteSpace(username)) return;

        var user = await _userManager.FindByNameAsync(username);
        if(user == null) throw new NullReferenceException("Usuário não encontrado! Por favor, tente novamente mais tarde!");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);
    }
}
