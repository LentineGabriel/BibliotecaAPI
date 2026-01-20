#region
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Roles;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    #region PROPS/CTOR
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _cfg;
    private readonly IMapper _mapper;

    public RolesController(ITokenService tokenService , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IConfiguration cfg , IMapper mapper)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _cfg = cfg;
        _mapper = mapper;
    }
    #endregion

    #region ROLES
    #region GET
    /// <summary>
    /// Retorna todos os perfis cadastrados no sistema.
    /// </summary>
    /// <returns>Todos os perfis de usuário</returns>
    // GET: /RolesController/Perfis
    [HttpGet]
    [Route("Perfis")]
    public async Task<ActionResult<IEnumerable<RolesResponseDTO>>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var result = _mapper.Map<IEnumerable<RolesResponseDTO>>(roles);

        return Ok(result);
    }

    /// <summary>
    /// Retorna um peril específico pelo ID.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /RolesController/Perfil/id
    [HttpGet]
    [Route("Perfil/{id}")]
    public async Task<ActionResult<IEnumerable<RolesResponseDTO>>> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role == null) return NotFound($"Não foi possível encontrar o perfil com o ID {id}. Por favor, tente novamente!");

        var roleDTO = _mapper.Map<RolesResponseDTO>(role);

        return Ok(roleDTO);
    }

    /// <summary>
    /// Retorna os usuários presentes em um perfil.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /RolesController/Perfil/id
    [HttpGet]
    [Route("UsuariosNoPerfil/{perfil}")]
    public async Task<ActionResult<IEnumerable<UsersResponseDTO>>> GetUsersInRoleAsync(string perfil)
    {
        var usuariosNoPerfil = await _userManager.GetUsersInRoleAsync(perfil);
        if(usuariosNoPerfil == null || !usuariosNoPerfil.Any()) return NotFound($"Nenhum usuário encontrado no perfil '{perfil}'. Por favor, tente novamente!");

        var usuariosNoPerfilDTO = _mapper.Map<IEnumerable<UsersResponseDTO>>(usuariosNoPerfil);
        return Ok(usuariosNoPerfilDTO);
    }
    #endregion

    #region POST
    /// <summary>
    /// Cria um novo perfil de usuário no sistema.
    /// </summary>
    /// <returns>Novo perfil de usuário</returns>
    // POST: /RolesController/CriarPerfil
    [HttpPost]
    [Route("CriarPerfil")]
    // [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if(!roleExists)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if(roleResult != null) return StatusCode(StatusCodes.Status200OK , new Response { Status = "Sucesso" , Message = $"Role '{roleName}' adicionada com sucesso!" });
            else return StatusCode(StatusCodes.Status400BadRequest , new Response { Status = "Erro" , Message = $"Erro ao adicionar a role '{roleName}'." });
        }
        return StatusCode(StatusCodes.Status400BadRequest , new Response { Status = "Erro" , Message = $"Role {roleName} já existe." });
    }

    /// <summary>
    /// Adiciona um usuário a um perfil do sistema.
    /// </summary>
    /// <returns>Usuário a um perfil</returns>
    // POST: /RolesController/AdicionarUsuarioAoPerfil
    [HttpPost]
    [Route("AdicionarUsuarioAoPerfil")]
    public async Task<IActionResult> AddUserToRole(string email , string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user != null)
        {
            var result = await _userManager.AddToRoleAsync(user , roleName);
            if(result.Succeeded) return StatusCode(StatusCodes.Status200OK , new Response { Status = "Sucesso" , Message = $"Usuário '{user.Email}' adicionado ao perfil '{roleName}'." });
            else return StatusCode(StatusCodes.Status400BadRequest , new Response { Status = "Erro" , Message = $"Erro ao adicionar o usuário '{user.Email}' ao perfil '{roleName}'." });
        }
        return BadRequest(new { Error = "Não foi possível encontrar o usuário. Por favor, tente novamente!" });
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza um perfil existente no sistema.
    /// </summary>
    /// <returns></returns>
    // PUT: /RolesController/AtualizarPerfil/id
    [HttpPut("AtualizarPerfil/{id}")]
    public async Task<ActionResult<RolesResponseDTO>> PutRoleAsync(string id , RolesRequestDTO rolesDTO)
    {
        if(id != rolesDTO.Id) return BadRequest($"Não foi possível encontrar o perfil com o nome '{id}'. Por favor, verifique o nome e tente novamente!");

        var role = await _roleManager.FindByIdAsync(id);
        if(role == null) return BadRequest($"Não foi possível encontrar o perfil com o nome '{id}'. Por favor, verifique o nome digitado e tente novamente!");

        role.Name = rolesDTO.Name;

        var result = await _roleManager.UpdateAsync(role);
        if(!result.Succeeded) return BadRequest(result.Errors);

        var response = _mapper.Map<RolesResponseDTO>(role);

        return Ok(response);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Deleta um perfil de usuário no sistema.
    /// </summary>
    /// <returns>Perfil de usuário deletado</returns>
    // DELETE: /RolesController/DeletarPerfil/RoleName
    [HttpDelete]
    [Route("DeletarPerfil/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role != null)
        {
            var result = await _roleManager.DeleteAsync(role);
            if(result.Succeeded) return StatusCode(StatusCodes.Status200OK , new Response { Status = "Sucesso" , Message = $"Perfil '{role.Name}' deletado com sucesso." });
            else return StatusCode(StatusCodes.Status400BadRequest , new Response { Status = "Erro" , Message = $"Erro ao deletar o perfil '{role.Name}'." });
        }
        return BadRequest(new { Error = "Não foi possível encontrar o perfil. Por favor, tente novamente!" });
    }
    #endregion
    #endregion
}
