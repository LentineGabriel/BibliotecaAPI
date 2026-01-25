#region USINGS
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.UsuariosFiltro;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using X.PagedList;
#endregion

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    #region PROPS/CTOR
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _cfg;
    private readonly IMapper _mapper;

    public UsersController(ITokenService tokenService , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IConfiguration cfg, IMapper mapper)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _cfg = cfg;
        _mapper = mapper;
    }
    #endregion

    #region USERS
    #region GET
    /// <summary>
    /// Retorna todos os usuários cadastrados no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios
    [HttpGet]
    [Route("Usuarios")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<IEnumerable<UsersResponseDTO>>> GetUsersAsync()
    {
        var usuarios = await _userManager.Users.ToListAsync();
        if(usuarios == null || !usuarios.Any()) return NotFound("Usuários não encontrados. Por favor, tente novamente!");

        var usuariosDTO = _mapper.Map<IEnumerable<UsersResponseDTO>>(usuarios);
        return Ok(usuariosDTO);
    }

    /// <summary>
    /// Retorna um usuário específico pelo ID.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/id
    [HttpGet]
    [Route("Usuarios/{id}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<UsersResponseDTO>> GetUserByIdAsync(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if(usuario == null) return NotFound($"Não foi possível encontrar o usuário com o ID {id}. Por favor, tente novamente!");

        var usuarioDTO = _mapper.Map<UsersResponseDTO>(usuario);

        return Ok(usuarioDTO);
    }

    /// <summary>
    /// Retorna os usuários cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Autores paginadas</returns>
    // GET: Usuarios/Paginacao
    [HttpGet("Paginacao")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<UsersResponseDTO>>> GetPaginationAsync([FromQuery] UsuariosParameters usuariosParameters)
    {
        var usuarios = await _userManager.Users.ToListAsync();
        if(usuarios == null || !usuarios.Any()) return NotFound("Usuários não encontrados. Por favor, tente novamente!");

        var usuariosPaged = await usuarios.ToPagedListAsync(usuariosParameters.PageNumber , usuariosParameters.PageSize);
        return ObterUsuarios(usuariosPaged);
    }
    #endregion

    #region LOGIN
    /// <summary>
    /// Retorna o usuário cadastrado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // POST: /AuthController/Login
    [HttpPost("LoginUsuario")]
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

            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role , userRole));
            }

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
    [HttpPost("RegistrarUsuario")]
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
        if(!result.Succeeded) return BadRequest(result.Errors);

        return Created(string.Empty , new Response { Status = "Successo" , Message = "Usuário criado com sucesso!" });
    }
    #endregion

    #region REFRESH TOKEN
    /// <summary>
    /// Cria um novo token de acesso usando o refresh token.
    /// </summary>
    /// <returns>Refresh Token</returns>
    // POST: /AuthController/RefreshToken
    [HttpPost("RefreshToken")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var accessToken = model.AccessToken;
        var refreshToken = model.RefreshToken;
        if(string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) return BadRequest("Token inválido.");

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        if(principal == null) return Unauthorized("Token inválido.");

        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if(string.IsNullOrWhiteSpace(userId)) return Unauthorized("Token inválido.");

        var user = await _userManager.FindByIdAsync(userId);
        if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) return Unauthorized("Token inválido ou expirado.");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = _tokenService.GetRefreshTokenExpiry();
        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken) ,
            RefreshToken = newRefreshToken
        });
    }
    #endregion

    #region REVOKE
    /// <summary>
    /// Revoga o token de um usuário.
    /// </summary>
    /// <returns></returns>
    // POST: /AuthController/Revoke
    [HttpPost("Revoke")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> Revoke(string username)
    {
        var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if(string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if(user == null) return NotFound("Usuário não encontrado");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza um usuário existente no sistema.
    /// </summary>
    /// <returns></returns>
    // PUT: /AuthController/AtualizarUsuario/id
    [HttpPut("AtualizarUsuario/{id}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<UsersResponseDTO>> PutAsync(string id, UsersRequestDTO usersDTO)
    {
        if(id != usersDTO.Id) return BadRequest($"Não foi possível encontrar o usuário com o ID {id}. Por favor, verifique o ID e tente novamente!");

        var user = await _userManager.FindByIdAsync(id);
        if(user == null) return BadRequest($"Não foi possível encontrar o usuário com ID {id}. Por favor, verifique o id digitado e tente novamente!");

        user.UserName = usersDTO.Username;
        user.Email = usersDTO.Email;
        user.EmailConfirmed = usersDTO.EmailConfirmed;

        var result = await _userManager.UpdateAsync(user);
        if(!result.Succeeded) return BadRequest(result.Errors);

        var response = _mapper.Map<UsersResponseDTO>(user);

        return Ok(response);
    }
    #endregion

    #region PATCH
    /// <summary>
    /// Atualiza partes de um usuário existente no sistema.
    /// </summary>
    /// <returns>Autor atualizado</returns>
    [HttpPatch("AtualizarParcialUsuario/{id}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<UsersResponseDTO>> PatchAsync(string id , [FromBody] JsonPatchDocument<UsersResponseDTO> patchDoc)
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");
        
        var user = await _userManager.FindByIdAsync(id);
        if(user == null) return BadRequest($"Não foi possível encontrar o usuário com ID {id}. Por favor, verifique o id digitado e tente novamente!");

        var userToPatch = _mapper.Map<UsersResponseDTO>(user);

        // Não deixando o EmailConfirmed ser alterado via patch
        if(patchDoc.Operations.Any(op => op.path.Equals("/emailConfirmed", StringComparison.OrdinalIgnoreCase))) return BadRequest("A propriedade 'EmailConfirmed' não pode ser alterada via patch.");

        patchDoc.ApplyTo(userToPatch , ModelState);
        if(!ModelState.IsValid) return BadRequest(ModelState);
        if(!TryValidateModel(userToPatch)) return BadRequest(ModelState);

        // Caso o email tenha sido alterado
        if(!string.Equals(userToPatch.Email, user.Email, StringComparison.OrdinalIgnoreCase)) 
        {
            var setEmail = await _userManager.SetEmailAsync(user, userToPatch.Email!);
            if(!setEmail.Succeeded) return BadRequest(setEmail.Errors);
        }

        // Caso o username tenha sido alterado
        if(!string.Equals(userToPatch.Username, user.UserName, StringComparison.OrdinalIgnoreCase)) 
        {
            var setUserName = await _userManager.SetUserNameAsync(user, userToPatch.Username!);
            if(!setUserName.Succeeded) return BadRequest(setUserName.Errors);
        }

        // P/ outros campos "não sensíveis"
        _mapper.Map(userToPatch , user);

        var result = await _userManager.UpdateAsync(user);
        if(!result.Succeeded) return BadRequest(result.Errors);

        var response = _mapper.Map<UsersResponseDTO>(user);

        return Ok(response);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Deleta um usuário do sistema.
    /// </summary>
    /// <returns>Usuário deletado</returns>
    // DELETE: /AuthController/DeletarUsuario/NomeUsuario
    [HttpDelete]
    [Route("DeletarUsuario/{id}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded) return StatusCode(StatusCodes.Status200OK , new Response { Status = "Sucesso", Message = $"Usuário '{user.UserName}' deletado com sucesso." });
            else return StatusCode(StatusCodes.Status400BadRequest , new Response { Status = "Erro", Message = $"Erro ao deletar o usuário '{user.UserName}'." });
        }
        return BadRequest(new { Error = "Não foi possível encontrar o usuário. Por favor, tente novamente!" });
    }
    #endregion
    #endregion

    #region METHODS
    private ActionResult<IEnumerable<UsersResponseDTO>> ObterUsuarios(IPagedList<ApplicationUser> usuarios)
    {
        var metadados = new
        {
            usuarios.Count ,
            usuarios.PageSize ,
            usuarios.PageCount ,
            usuarios.TotalItemCount ,
            usuarios.HasNextPage ,
            usuarios.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination" , JsonConvert.SerializeObject(metadados));

        var usuariosDTO = _mapper.Map<IEnumerable<UsersResponseDTO>>(usuarios);
        return Ok(usuariosDTO);
    }
    #endregion
}