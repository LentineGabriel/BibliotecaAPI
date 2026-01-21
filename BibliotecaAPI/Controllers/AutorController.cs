#region USINGS
using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.AutoresFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
#endregion

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AutorController : ControllerBase
{
    #region PROPS/CTORS
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;
    
    public AutorController(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
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
        var autores = await _uof.AutorRepositorio.GetAllAsync();
        if(autores == null || !autores.Any()) return NotFound("Autores não encontrados. Por favor, tente novamente!");

        var autoresDTO = _mapper.Map<IEnumerable<AutorDTOResponse>>(autores);
        return Ok(autoresDTO);
    }

    /// <summary>
    /// Retorna um autor específico pelo ID.
    /// </summary>
    /// <returns>Autor encontrado</returns>
    // GET: Autor/{id}
    [HttpGet("{id:int:min(1)}", Name = "ObterIdAutor")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> GetByIdAsync(int id)
    {
        var autor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(autor == null) return NotFound($"Autor por ID = {id} não encontrado. Por favor, tente novamente!");
        
        var autorDTO = _mapper.Map<AutorDTOResponse>(autor);
        return Ok(autorDTO);
    }

    /// <summary>
    /// Retorna os autores cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Autores paginadas</returns>
    // GET: Autor/Paginacao
    [HttpGet("Paginacao")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetPaginationAsync([FromQuery] AutoresParameters autoresParameters)
    {
        var autores = await _uof.AutorRepositorio.GetAutoresAsync(autoresParameters);
        return ObterAutores(autores);
    }

    /// <summary>
    /// Retorna os autores filtrando pelo seu nome (via paginação).
    /// </summary>
    /// <returns>Autor encontrado pelo seu nome</returns>
    // GET: Autor/PesquisaPorNome
    [HttpGet("PesquisaPorNome")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetFilterNamePaginationAsync([FromQuery] AutoresFiltroNome autoresFiltroNome)
    {
        var autores = await _uof.AutorRepositorio.GetAutoresFiltrandoPeloNome(autoresFiltroNome);
        return ObterAutores(autores);
    }

    /// <summary>
    /// Retorna um autor filtrando pela nacionalidade (via paginação).
    /// </summary>
    /// <returns>Autor encontrado por sua nacionalidade</returns>
    // GET: Autor/PesquisaPorNacionalidade
    [HttpGet("PesquisaPorNacionalidade")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetFilterNationalityPaginationAsync([FromQuery] AutoresFiltroNacionalidade autoresFiltroNacionalidade)
    {
        var autores = await _uof.AutorRepositorio.GetAutoresFiltrandoPelaNacionalidade(autoresFiltroNacionalidade);
        return ObterAutores(autores);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona um novo autor ao sistema.
    /// </summary>
    /// <returns>Autor criado</returns>
    [HttpPost("AdicionarAutores")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<AutorDTOResponse>> PostAsync(AutorDTORequest autorDTO)
    {
        if(autorDTO == null) return BadRequest("Não foi possível adicionar um novo autor. Tente novamente mais tarde!");

        var autorNovo = _mapper.Map<Autor>(autorDTO);
        var autorCriado = _uof.AutorRepositorio.Create(autorNovo);
        await _uof.CommitAsync();

        var autorRetornoDTO = _mapper.Map<AutorDTOResponse>(autorCriado);
        return new CreatedAtRouteResult("ObterIdAutor" , new { id = autorCriado.IdAutor } , autorRetornoDTO);
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

        var autor = _mapper.Map<Autor>(autorDTO);
        var autorExistente = _uof.AutorRepositorio.Update(autor);
        await _uof.CommitAsync();

        var autorRetornoDTO = _mapper.Map<AutorDTOResponse>(autorExistente);
        return Ok(autorRetornoDTO);
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

        var autor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(autor == null) return NotFound($"Autor com ID {id} não encontrado. Por favor, verifique o ID e tente novamente!");

        var autorDTO = _mapper.Map<AutorDTORequest>(autor);
        patchDoc.ApplyTo(autorDTO , ModelState);
        if(!ModelState.IsValid) return BadRequest(ModelState);

        _mapper.Map(autorDTO , autor);
        _uof.AutorRepositorio.Update(autor);
        await _uof.CommitAsync();

        var autorRetornoDTO = _mapper.Map<AutorDTOResponse>(autor);
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
        var deletarAutor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(deletarAutor == null) return NotFound("Autor não localizado! Verifique o ID digitado");

        var autorExcluido = _uof.AutorRepositorio.Delete(deletarAutor);
        await _uof.CommitAsync();

        var autorExcluidoDTO = _mapper.Map<AutorDTOResponse>(autorExcluido);
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
