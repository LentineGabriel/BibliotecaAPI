#region USINGS
using Asp.Versioning;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.EstanteUC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
#endregion

namespace BibliotecaAPI.Controllers.V2;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
[ApiVersion("2.0")]
public class UsersController : ControllerBase
{
    #region PROPS/CTOR
    private readonly IGetUsersUseCase _getUsersUseCase;
    private readonly IGetLivroEstante _getLivroEstante;
    private readonly ICreateLivroEstante _createLivroEstante;

    public UsersController(IGetUsersUseCase getUsersUseCase , IGetLivroEstante getLivroEstante , ICreateLivroEstante createLivroEstante)
    {
        _getUsersUseCase = getUsersUseCase;
        _getLivroEstante = getLivroEstante;
        _createLivroEstante = createLivroEstante;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna a estante do usuário logado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/Id/Estante
    [HttpGet("Estante")]
    public async Task<ActionResult<IEnumerable<Estante>>> GetUserEstanteAsync(int page = 1, int pageSize = 10)
    {
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(usuarioId == null) return Unauthorized("É necessário estar logado para visualizar a estante.");

        var estante = await _getLivroEstante.GetAsync(usuarioId , page , pageSize); // ignorar essa possível referencia nula ou dará erro 500 para o usuário (mesmo que funcione)

        return Ok(estante);
    }
    #endregion

    #region POST
    [HttpPost("Estante")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<Estante>> CreateUserEstanteAsync(int livroId)
    {
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(usuarioId == null) return Unauthorized("É necessário estar logado para mexer na estante.");

        var estante = await _createLivroEstante.CreateAsync(usuarioId , livroId); // ignorar essa possível referencia nula ou dará erro 500 para o usuário (mesmo que funcione)

        return Ok(estante);
    }

    [HttpPost("BuscarLivros")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<UsersResponseDTO>> BooksSearchAsync(string id)
    {
        // var usuarioId = await _getUsersUseCase.BuscarLivrosAsync(id);
        return Ok();
    }
    #endregion
}
