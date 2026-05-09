#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.DTOs.EstanteDTOs;
using BibliotecaAPI.Extensions;
using BibliotecaAPI.Models;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.EstanteUC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using X.PagedList;
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
    private readonly IMapper _mapper;

    public UsersController(IGetUsersUseCase getUsersUseCase , IGetLivroEstante getLivroEstante , ICreateLivroEstante createLivroEstante , IMapper mapper)
    {
        _getUsersUseCase = getUsersUseCase;
        _getLivroEstante = getLivroEstante;
        _createLivroEstante = createLivroEstante;
        _mapper = mapper;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna a estante do usuário logado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/Estante
    [HttpGet("Estante")]
    public async Task<ActionResult<IEnumerable<EstanteDTOResponse>>> GetUserEstanteAsync(int page = 1, int pageSize = 10)
    {
        var usuarioId = User.GetUserId();
        if (usuarioId == null) return Unauthorized("É necessário estar logado para visualizar a estante.");

        var estanteEnumerable = await _getLivroEstante.GetAsync(usuarioId, page, pageSize);

        // Verifica se o resultado já é um IPagedList, caso contrário, converte para PagedList
        var estantePaged = estanteEnumerable as IPagedList<Estante> ?? estanteEnumerable.ToPagedList(page, pageSize);

        return ObterEstante(estantePaged);
    }

    /// <summary>
    /// Retorna um livro da estante do usuário logado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/Estante/BuscarLivros
    [HttpGet("Estante/BuscarLivros")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<EstanteDTOResponse>>> BooksSearchAsync(string termo, int page = 1 , int pageSize = 10)
    {
        var usuarioId = User.GetUserId();
        if(usuarioId == null) return Unauthorized("É necessário estar logado para mexer na estante.");

        var buscaEnumerable = await _getLivroEstante.SearchBooksAsync(usuarioId, page, pageSize, termo);

        // Verifica se o resultado já é um IPagedList, caso contrário, converte para PagedList
        var buscaPaged = buscaEnumerable as IPagedList<Estante> ?? buscaEnumerable.ToPagedList(page , pageSize);

        return ObterEstante(buscaPaged);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona um livro a estante do usuário logado no sistema.
    /// </summary>
    /// <returns>Usuário cadastrado</returns>
    // GET: /AuthController/Usuarios/Estante/AdicionarLivro
    [HttpPost("Estante/AdicionarLivro")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<EstanteDTOResponse>> CreateUserEstanteAsync(int livroId)
    {
        var usuarioId = User.GetUserId();
        if(usuarioId == null) return Unauthorized("É necessário estar logado para mexer na estante.");

        var estante = await _createLivroEstante.CreateAsync(usuarioId , livroId);

        return Ok(estante);
    }
    #endregion

    #region METHODS
    private ActionResult<IEnumerable<EstanteDTOResponse>> ObterEstante(IPagedList<Estante> estante)
    {
        var metadados = new
        {
            estante.Count,
            estante.PageSize,
            estante.PageCount,
            estante.TotalItemCount,
            estante.HasNextPage,
            estante.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadados));

        var estanteDTO = _mapper.Map<IEnumerable<EstanteDTOResponse>>(estante);
        return Ok(estanteDTO);
    }
    #endregion
}
