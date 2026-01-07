using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    #region PROPS/CTOR
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _cfg;

    public AuthController(ITokenService tokenService , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IConfiguration cfg)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _cfg = cfg;
    }
    #endregion

    #region LOGIN
    /// <summary>
    /// Retorna o usuário cadastrado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // POST: /AuthController/Login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.FindByNameAsync(model.NomeUsuario!);
        if(user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // guid id
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), // id usuário
                new Claim(ClaimTypes.Name, user.UserName!), // nome usuário
                new Claim(ClaimTypes.Email, user.Email!), // email usuário
            };

            //foreach(var userRole in userRoles)
            //{
            //    authClaims.Add(new Claim(ClaimTypes.Role , userRole));
            //}

            var token = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Salvar refresh token no banco de dados
            if(!int.TryParse(_cfg["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes) || refreshTokenValidityInMinutes <= 0)
                throw new InvalidOperationException("Configuração inválida para validade do refresh token.");

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInMinutes);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        return Unauthorized("Credenciais inválidas");
    }
    #endregion

    #region REGISTER
    /// <summary>
    /// Cria o usuário no sistema.
    /// </summary>
    /// <returns>Cadastro do usuário criado</returns>
    // POST: /AuthController/Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        if(await _userManager.FindByNameAsync(model.NomeUsuario!) != null) return Conflict(new Response { Status = "Error" , Message = "Usuário já existe!" });
        if(await _userManager.FindByEmailAsync(model.EmailUsuario!) != null) return Conflict(new Response { Status = "Error" , Message = "Email já está em uso!" });

        var user = new ApplicationUser
        {
            UserName = model.NomeUsuario!.Trim() ,
            Email = model.EmailUsuario!.Trim().ToLowerInvariant() ,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user , model.Password!);
        if(!result.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError , new Response { Status = "Error" , Message = "Falha ao criar o usuário!" });

        return Created(string.Empty , new Response { Status = "Success" , Message = "Usuário criado com sucesso!" });
    }
    #endregion

    #region REFRESH TOKEN
    /// <summary>
    /// Cria um novo token de acesso usando o refresh token.
    /// </summary>
    /// <returns>Refresh Token</returns>
    // POST: /AuthController/RefreshToken
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(TokenModel model)
    {
        if(model is null) return BadRequest("Requisição inválida!");

        // obtém os tokens expirados e suas claims
        string? accessToken = model.AccessToken ?? throw new ArgumentException(nameof(model));
        string? refreshToken = model.RefreshToken ?? throw new ArgumentException(nameof(model));
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        if(principal == null) return BadRequest("Token de Acesso/Refresh Token inválido(s)!");

        string? username = principal.Identity?.Name;
        var user = await _userManager.FindByNameAsync(username!);
        if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) return BadRequest("Token de Acesso/Refresh Token inválido(s)!");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // atualiza o refresh token no banco de dados
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new 
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken) ,
            refreshToken = newRefreshToken,
        });
    }
    #endregion

    #region REVOKE
    /// <summary>
    /// Revoga o token do usuário.
    /// </summary>
    /// returns>Revoke Token</returns>
    // POST: /AuthController/Revoke
    [HttpPost("Revoke/{username}")]
    [Authorize]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if(user == null) return NotFound("Usuário não encontrado!");

        // revoga o refresh token
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
    #endregion
}
