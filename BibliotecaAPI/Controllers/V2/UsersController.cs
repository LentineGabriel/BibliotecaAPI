#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using Microsoft.AspNetCore.Mvc;
#endregion

namespace BibliotecaAPI.Controllers.V2;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
[ApiVersion("2.0")]
public class UsersController : ControllerBase
{
    #region PROPS/CTOR
    private readonly IGetUsersUseCase _getUsersUseCase;

    public UsersController(IGetUsersUseCase getUsersUseCase)
    {
        _getUsersUseCase = getUsersUseCase;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna a estante do usuário logado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/Id/Estante
    [HttpGet("{id}/Estante")]
    public async Task<ActionResult<UsersResponseDTO>> GetUserEstanteAsync(string id)
    {
        // var usuarioId = await _getUsersUseCase.GetUserEstanteAsync(id);
        return Ok();
    }
    #endregion

    #region POST
    [HttpPost("BuscarLivros")]
    public async Task<ActionResult<UsersResponseDTO>> BooksSearchAsync(string id)
    {
        // var usuarioId = await _getUsersUseCase.BuscarLivrosAsync(id);
        return Ok();
    }
    #endregion
}
