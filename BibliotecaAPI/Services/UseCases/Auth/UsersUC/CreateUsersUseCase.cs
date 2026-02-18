using AutoMapper;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.TokenJWT;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BibliotecaAPI.Services.UseCases.Auth.UsersUC;

public class CreateUsersUseCase : ICreateUsersUseCase
{
    #region PROS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _cfg;

    public CreateUsersUseCase(UserManager<ApplicationUser> userManager , IMapper mapper , ITokenService tokenService , IConfiguration cfg)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _cfg = cfg;
    }
    #endregion

    public async Task<IdentityResult> Login(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.NomeUsuario!);
        if(user is not null && await _userManager.CheckPasswordAsync(user , model.Password!))
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

            var token = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Salvar refresh token no banco de dados
            if(!int.TryParse(_cfg["JWT:RefreshTokenValidityInMinutes"] , out int refreshTokenValidityInMinutes) || refreshTokenValidityInMinutes <= 0)
                throw new InvalidOperationException("Configuração inválida para validade do refresh token.");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInMinutes);

            var usuarioLogado = await _userManager.UpdateAsync(user);
            return usuarioLogado;
        }

        throw new UnauthorizedAccessException("Credenciais inválidas");
    }

    public async Task<IdentityResult> Register(RegisterModel model)
    {
        if(await _userManager.FindByNameAsync(model.NomeUsuario!) != null) return IdentityResult.Failed(new IdentityError { Code = "UserExists" , Description = "Usuário já existe!" });
        if(await _userManager.FindByEmailAsync(model.EmailUsuario!) != null) return IdentityResult.Failed(new IdentityError { Code = "EmailInUse" , Description = "Email já está em uso!" });

        var user = new ApplicationUser
        {
            UserName = model.NomeUsuario!.Trim() ,
            Email = model.EmailUsuario!.Trim().ToLowerInvariant() ,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user , model.Password!);
        return result;
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

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
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
