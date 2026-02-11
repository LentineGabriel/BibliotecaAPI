#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Roles;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.DTOs.TokensJWT;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.PerfilFiltro;
using BibliotecaAPI.Services.Interfaces.Auth.RolesUC;
using BibliotecaAPI.Services.Interfaces.TokenJWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;
#endregion

namespace BibliotecaAPI.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
[ApiVersion("1.0")]
public class RolesController : ControllerBase
{
    #region PROPS/CTOR
    private readonly IGetRolesUseCase _getRolesUseCase;
    private readonly ICreateRolesUseCase _createRolesUseCase;
    private readonly IMapper _mapper;

    public RolesController(IGetRolesUseCase getRolesUseCase , IMapper mapper , ICreateRolesUseCase createRolesUseCase)
    {
        _getRolesUseCase = getRolesUseCase;
        _mapper = mapper;
        _createRolesUseCase = createRolesUseCase;
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
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<IEnumerable<RolesResponseDTO>>> GetRolesAsync()
    {
        var result = await _getRolesUseCase.GetRolesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retorna um peril específico pelo ID.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /RolesController/Perfil/id
    [HttpGet("{id}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<IEnumerable<RolesResponseDTO>>> GetRoleByIdAsync(string id)
    {
        var roleId = await _getRolesUseCase.GetRoleByIdAsync(id);
        return Ok(roleId);
    }

    /// <summary>
    /// Retorna os usuários presentes em um perfil.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /RolesController/Perfil/id
    [HttpGet("UsuariosNoPerfil/{perfil}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<IEnumerable<UsersResponseDTO>>> GetUsersInRoleAsync(string perfil)
    {
        var usuariosNoPerfil = await _getRolesUseCase.GetUsersInRoleAsync(perfil);
        return Ok(usuariosNoPerfil);
    }

    /// <summary>
    /// Retorna os perfis cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Autores paginadas</returns>
    // GET: Perfil/Paginacao
    [HttpGet("Paginacao")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<RolesResponseDTO>>> GetPaginationAsync([FromQuery] PerfilParameters perfilParameters)
    {
        var perfisPaginados = await _getRolesUseCase.GetPaginationAsync(perfilParameters);
        return ObterPerfisRolesResponseDTO(perfisPaginados);
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
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var perfilCriado = await _createRolesUseCase.CreateRole(roleName);
        return Ok(perfilCriado);
    }

    /// <summary>
    /// Adiciona um usuário a um perfil do sistema.
    /// </summary>
    /// <returns>Usuário a um perfil</returns>
    // POST: /RolesController/AdicionarUsuarioAoPerfil
    [HttpPost]
    [Route("AdicionarUsuarioAoPerfil")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> AddUserToRole(string email , string roleName)
    {
        var usuarioNoPerfil = await _createRolesUseCase.AddUserToRole(email , roleName);
        return Ok(usuarioNoPerfil);
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza um perfil existente no sistema.
    /// </summary>
    /// <returns></returns>
    // PUT: /RolesController/AtualizarPerfil/id
    [HttpPut("AtualizarPerfil/{id}")]
    [Authorize(Policy = "AdminsOnly")]
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
    [Authorize(Policy = "AdminsOnly")]
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

    #region METHODS
    private ActionResult<IEnumerable<RolesResponseDTO>> ObterPerfisIdentityRole(IPagedList<IdentityRole> perfil)
    {
        var metadados = new
        {
            perfil.Count ,
            perfil.PageSize ,
            perfil.PageCount ,
            perfil.TotalItemCount ,
            perfil.HasNextPage ,
            perfil.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination" , JsonConvert.SerializeObject(metadados));

        var perfilDTO = _mapper.Map<IEnumerable<RolesResponseDTO>>(perfil);
        return Ok(perfilDTO);
    }

    private ActionResult<IEnumerable<RolesResponseDTO>> ObterPerfisRolesResponseDTO(IPagedList<RolesResponseDTO> perfil)
    {
        var metadados = new
        {
            perfil.Count ,
            perfil.PageSize ,
            perfil.PageCount ,
            perfil.TotalItemCount ,
            perfil.HasNextPage ,
            perfil.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination" , JsonConvert.SerializeObject(metadados));
        return Ok(perfil);
    }
    #endregion
}
