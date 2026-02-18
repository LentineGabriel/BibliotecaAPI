#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.UsuariosFiltro;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.TokenJWT;
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

namespace BibliotecaAPI.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    #region PROPS/CTOR
    private readonly IGetUsersUseCase _getUsersUseCase;
    private readonly ICreateUsersUseCase _createUsersUseCase;
    private readonly IMapper _mapper;

    public UsersController(IGetUsersUseCase getUsersUseCase , ICreateUsersUseCase createUsersUseCase , IMapper mapper)
    {
        _getUsersUseCase = getUsersUseCase;
        _createUsersUseCase = createUsersUseCase;
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
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<IEnumerable<UsersResponseDTO>>> GetUsersAsync()
    {
        var usuario = await _getUsersUseCase.GetUsersAsync();
        return Ok(usuario);
    }

    /// <summary>
    /// Retorna um usuário específico pelo ID.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/id
    [HttpGet("{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<UsersResponseDTO>> GetUserByIdAsync(string id)
    {
        var usuarioId = await _getUsersUseCase.GetUserByIdAsync(id);
        return Ok(usuarioId);
    }

    /// <summary>
    /// Retorna os usuários cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Autores paginadas</returns>
    // GET: Usuarios/Paginacao
    [HttpGet("Paginacao")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<UsersResponseDTO>>> GetPaginationAsync([FromQuery] UsuariosParameters usuariosParameters)
    {
        var usuariosPaginados = await _getUsersUseCase.GetPaginationAsync(usuariosParameters);
        return ObterUsuarios(usuariosPaginados);
    }
    #endregion

    #region LOGIN
    /// <summary>
    /// Retorna o usuário cadastrado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // POST: /AuthController/Login
    [HttpPost("LoginUsuario")]
    [ApiVersion("1.0")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var usuarioLogado = await _createUsersUseCase.Login(model);
        return Ok(usuarioLogado);
    }
    #endregion

    #region REGISTER
    /// <summary>
    /// Cria o usuário no sistema.
    /// </summary>
    /// <returns>Cadastro do usuário criado</returns>
    // POST: /AuthController/Register
    [HttpPost("RegistrarUsuario")]
    [ApiVersion("1.0")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        await _createUsersUseCase.Register(model);

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
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        await _createUsersUseCase.RefreshToken(model);

        return Ok(new Response { Status = "Sucesso", Message = "Refresh token processado." });
    }
    #endregion

    #region REVOKE
    /// <summary>
    /// Revoga o token de um usuário.
    /// </summary>
    /// <returns></returns>
    // POST: /AuthController/Revoke
    [HttpPost("Revoke")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> Revoke(string username)
    {
        await _createUsersUseCase.Revoke(username);
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
    [ApiVersion("1.0")]
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
    [ApiVersion("1.0")]
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
    [ApiVersion("1.0")]
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

    private ActionResult<IEnumerable<UsersResponseDTO>> ObterUsuarios(IPagedList<UsersResponseDTO> usuarios)
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