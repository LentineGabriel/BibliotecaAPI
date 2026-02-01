#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.AutoresFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Autores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
#endregion

namespace BibliotecaAPI.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
[ApiVersion("1.0")]
public class AutorController : ControllerBase
{
    #region PROPS/CTORS
    private readonly IMapper _mapper;
    private readonly IGetAutoresUseCase _getAutoresUseCase;
    private readonly ICreateAutoresUseCase _createAutoresUseCase;
    private readonly IPutAutoresUseCase _putAutoresUseCase;
    private readonly IPatchAutoresUseCase _patchAutoresUseCase;
    private readonly IDeleteAutoresUseCase _deleteAutoresUseCase;

    public AutorController(IMapper mapper, IGetAutoresUseCase getAutoresUseCase, ICreateAutoresUseCase createAutoresUseCase, IPutAutoresUseCase putAutoresUseCase, IPatchAutoresUseCase patchAutoresUseCase, IDeleteAutoresUseCase deleteAutoresUseCase)
    {
        _mapper = mapper;
        _getAutoresUseCase = getAutoresUseCase;
        _createAutoresUseCase = createAutoresUseCase;
        _putAutoresUseCase = putAutoresUseCase;
        _patchAutoresUseCase = patchAutoresUseCase;
        _deleteAutoresUseCase = deleteAutoresUseCase;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna todos os autores cadastrados no sistema.
    /// </summary>
    /// <returns>Lista de Autores</returns>
    // GET: Autor
    [HttpGet]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<AutorDTOResponse>>> GetAsync()
    {
        var autores = await _getAutoresUseCase.GetAllAsync();
        return Ok(autores);
    }

    /// <summary>
    /// Retorna um autor específico pelo ID.
    /// </summary>
    /// <returns>Autor encontrado</returns>
    // GET: Autor/{id}
    [HttpGet("AutoresPorId/{id:int:min(1)}")]
    //[Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> GetByIdAsync(int id)
    {
        var autorId = await _getAutoresUseCase.GetByIdAsync(id);
        return Ok(autorId);
    }

    /// <summary>
    /// Retorna os autores cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Autores paginadas</returns>
    // GET: Autor/Paginacao
    [HttpGet]
    [Route("Paginacao")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetPaginationAsync([FromQuery] AutoresParameters autoresParameters)
    {
        var autores = await _getAutoresUseCase.GetPaginationAsync(autoresParameters);
        return ObterAutores(autores);
    }

    /// <summary>
    /// Retorna os autores filtrando pelo seu nome (via paginação).
    /// </summary>
    /// <returns>Autor encontrado pelo seu nome</returns>
    // GET: Autor/PesquisaPorNome
    [HttpGet]
    [Route("PesquisaPoNome")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetFilterNamePaginationAsync([FromQuery] AutoresFiltroNome autoresFiltroNome)
    {
        var autores = await _getAutoresUseCase.GetFilterNamePaginationAsync(autoresFiltroNome);
        return ObterAutores(autores);
    }

    /// <summary>
    /// Retorna um autor filtrando pela nacionalidade (via paginação).
    /// </summary>
    /// <returns>Autor encontrado por sua nacionalidade</returns>
    // GET: Autor/PesquisaPorNacionalidade
    [HttpGet]
    [Route("PesquisaPorNacionalidade")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetFilterNationalityPaginationAsync([FromQuery] AutoresFiltroNacionalidade autoresFiltroNacionalidade)
    {
        var autores = await _getAutoresUseCase.GetFilterNationalityPaginationAsync(autoresFiltroNacionalidade);
        return ObterAutores(autores);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona um novo autor ao sistema.
    /// </summary>
    /// <returns>Autor criado</returns>
    [HttpPost]
    [Route("AdicionarAutores")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> PostAsync([FromBody] AutorDTORequest autorDTO)
    {
        if(autorDTO == null) return BadRequest("Não foi possível adicionar um novo autor. Tente novamente mais tarde!");
        var autorCriado = await _createAutoresUseCase.CreateAsync(autorDTO);

        return Ok(autorCriado);
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza um autor existente no sistema.
    /// </summary>
    /// <returns>Autor atualizado</returns>
    [HttpPut("AtualizarAutor/{id:int:min(1)}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> PutAsync(int id , AutorDTORequest autorDTO)
    {
        if(id != autorDTO.IdAutor) return BadRequest($"Não foi possível encontrar o autor com ID {id}. Por favor, verifique o ID e tente novamente!");
        var autorExistente = await _putAutoresUseCase.PutAsync(autorDTO);

        return Ok(autorExistente);
    }
    #endregion

    #region PATCH
    /// <summary>
    /// Atualiza partes de um autor existente no sistema.
    /// </summary>
    /// <returns>Autor atualizado</returns>
    [HttpPatch("AtualizarParcialAutor/{id:int:min(1)}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> PatchAsync(int id , JsonPatchDocument<AutorDTORequest> patchDoc)
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");
        var autorRetornoDTO = await _patchAutoresUseCase.PatchAsync(id , patchDoc);

        return Ok(autorRetornoDTO);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Deleta um autor existente no sistema.
    /// </summary>
    /// <returns>Autor deletado</returns>
    [HttpDelete("DeletarAutor/{id:int:min(1)}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> DeleteAsync(int id)
    {
        var autorExcluidoDTO =  await _deleteAutoresUseCase.DeleteAsync(id);
        return Ok(autorExcluidoDTO);
    }
    #endregion

    #region METHODS
    private ActionResult<IEnumerable<Autor>> ObterAutores(IPagedList<Autor> autores)
    {
        var metadados = new
        {
            autores.Count ,
            autores.PageSize ,
            autores.PageCount ,
            autores.TotalItemCount ,
            autores.HasNextPage ,
            autores.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination" , JsonConvert.SerializeObject(metadados));

        var autoresDTO = _mapper.Map<IEnumerable<AutorDTOResponse>>(autores);
        return Ok(autoresDTO);
    }
    #endregion
}
