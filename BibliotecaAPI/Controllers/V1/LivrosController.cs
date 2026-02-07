#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.LivrosFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Livros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;
#endregion

namespace BibliotecaAPI.Controllers.V1;

[ApiController]
[Route("[controller]")]
public class LivrosController : ControllerBase
{
    #region PROPS/CTORS
    private readonly IMapper _mapper;
    private readonly IGetLivrosUseCase _getLivrosUseCase;
    private readonly ICreateLivrosUseCase _createLivrosUseCase;
    private readonly IPutLivrosUseCase _putLivrosUseCase;
    private readonly IPatchLivrosUseCase _patchLivrosUseCase;

    public LivrosController(IMapper mapper, IGetLivrosUseCase getLivrosUseCase, ICreateLivrosUseCase createLivrosUseCase, IPutLivrosUseCase putLivrosUseCase, IPatchLivrosUseCase patchLivrosUseCase)
    {
        _mapper = mapper;
        _getLivrosUseCase = getLivrosUseCase;
        _createLivrosUseCase = createLivrosUseCase;
        _putLivrosUseCase = putLivrosUseCase;
        _patchLivrosUseCase = patchLivrosUseCase;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna todas os livros cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de editoras</returns>
    // GET: /Livros
    [HttpGet]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<LivrosDTOResponse>>> GetAsync()
    {
        var livros = await _getLivrosUseCase.GetAsync();
        if(livros == null || !livros.Any()) return NotFound("Livros não encontrados. Por favor, tente novamente!");

        return Ok(livros);
    }

    /// <summary>
    /// Retorna um livro específico pelo ID.
    /// </summary>
    /// <returns>Livro via ID</returns>
    // GET: /Livros/{id}
    [HttpGet("{id:int:min(1)}", Name = "ObterIdLivro")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<LivrosDTOResponse>> GetByIdAsync(int id)
    {
        var livroId = await _getLivrosUseCase.GetByIdAsync(id);
        if(livroId == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");
        
        return Ok(livroId);
    }

    /// <summary>
    /// Retorna os livros cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Livros paginadas</returns>
    // GET: /Livros/Paginacao
    [HttpGet("Paginacao")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Livros>>> GetPaginationAsync([FromQuery] LivrosParameters livrosParameters)
    {
        var livrosPaginados = await _getLivrosUseCase.GetPaginationAsync(livrosParameters);
        return ObterLivros(livrosPaginados);
    }

    /// <summary>
    /// Retorna os livros filtrando pelo nome (via paginação).
    /// </summary>
    /// <returns>Livros por nome</returns>
    // GET: /Livros/PesquisaPorNome
    [HttpGet("PesquisaPorNome")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Livros>>> GetFilterByNameAsync([FromQuery] LivrosFiltroNome livrosFiltroNome)
    {
        var livros = await _getLivrosUseCase.GetFilterByNameAsync(livrosFiltroNome);
        return ObterLivros(livros);
    }

    /// <summary>
    /// Retorna os livros filtrando pelo nome de seu autor (via paginação).
    /// </summary>
    /// <returns>Livros por nome do autor</returns>
    // GET: /Livros/PesquisaPorAutor
    [HttpGet("PesquisaPorAutor")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Livros>>> GetFilterByAutorAsync([FromQuery] LivrosFiltroAutor livrosFiltroAutor)
    {
        var livros = await _getLivrosUseCase.GetFilterByAutorAsync(livrosFiltroAutor);
        return ObterLivros(livros);
    }

    /// <summary>
    /// Retorna os livros filtrando pelo nome de sua editora (via paginação).
    /// </summary>
    /// <returns>Livros por nome da editora</returns>
    // GET: /Livros/PesquisaPorEditora
    [HttpGet("PesquisaPorEditora")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Livros>>> GetFilterByEditoraAsync([FromQuery] LivrosFiltroEditora livrosFiltroEditora)
    {
        var livros = await _getLivrosUseCase.GetFilterByEditoraAsync(livrosFiltroEditora);
        return ObterLivros(livros);
    }

    /// <summary>
    /// Retorna os livros filtrando pelo ano de publicação (via paginação).
    /// </summary>
    /// <returns>Livros pelo ano de publicação</returns>
    // GET: /Livros/PesquisaPorAnoPublicacao
    [HttpGet("PesquisaPorAnoPublicacao")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Livros>>> GetFilterByAnoPublicacaoAsync([FromQuery] LivrosFiltroAnoPublicacao livrosFiltroAnoPublicacao)
    {
        var livros = await _getLivrosUseCase.GetFilterByAnoPublicacaoAsync(livrosFiltroAnoPublicacao);
        return ObterLivros(livros);
    }

    /// <summary>
    /// Retorna os livros filtrando pelo seu gênero (via paginação).
    /// </summary>
    /// <returns>Livros pelo seu gênero</returns>
    // GET: /Livros/PesquisaPorCategoria
    [HttpGet("PesquisaPorCategoria")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Livros>>> GetFilterByCategoriaAsync([FromQuery] LivrosFiltroCategoria livrosFiltroCategoria)
    {
        var livros = await _getLivrosUseCase.GetFilterByCategoriaAsync(livrosFiltroCategoria);
        return ObterLivros(livros);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona um novo livro no sistema.
    /// </summary>
    /// <returns></returns>
    [HttpPost("AdicionarLivro")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<LivrosDTOResponse>> PostAsync(LivrosDTORequest livrosDTO)
    {
        if(livrosDTO == null) return BadRequest("Não foi possível adicionar um novo livro. Tente novamente mais tarde!");
        var livroCriado = _createLivrosUseCase.PostAsync(livrosDTO);

        return Ok(livroCriado);
    }

    // Livros lidos por um usuário - VERSÃO 2.0
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza um livro existente no sistema.
    /// </summary>
    /// <returns></returns>
    [HttpPut("AtualizarLivro/{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<LivrosDTOResponse>> PutAsync(int id, LivrosDTORequest livrosDTO)
    {
        if(id != livrosDTO.IdLivro) return BadRequest($"Não foi possível encontrar o livro com ID {id}. Por favor, verifique o ID digitado e tente novamente!");
        var livroAtualizado = await _putLivrosUseCase.PutAsync(id, livrosDTO);

        return Ok(livroAtualizado);
    }
    #endregion

    #region PATCH
    /// <summary>
    /// Atualiza partes de um livro existente no sistema.
    /// </summary>
    /// <returns>Livro atualizada</returns>
    [HttpPatch("AtualizarParcialLivro/{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<LivrosDTOResponse>> PatchAsync(int id , JsonPatchDocument<LivrosDTORequest> patchDoc) 
    {
        var livroAtualizado = await _patchLivrosUseCase.PatchAsync(id, patchDoc);
        return Ok(livroAtualizado);
    }

    /// <summary>
    /// Atualiza as categorias para os livros do sistema.
    /// </summary>
    /// <returns>Categoria(s) do Livro atualizada</returns>
    [HttpPatch("AtualizarCategorias/{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<IActionResult> PatchCategoriasAsync(int id, [FromBody] LivrosCategoriasPatchDTO dto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var livro = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        if(livro == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");

        // limpando todas as categorias presentes
        if(!dto.IdsCategorias.Any()) livro.LivrosCategorias!.Clear();

        foreach(var categoriaId in dto.IdsCategorias)
        {
            livro.LivrosCategorias!.Add(new LivroCategoria
            {
                LivroId = livro.IdLivro ,
                CategoriaId = categoriaId
            });
        }

        _uof.LivrosRepositorio.Update(livro);
        await _uof.CommitAsync();

        return Ok(new { Message = "Categorias atualizadas com sucesso!" });
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Deleta um livro existente no sistema.
    /// </summary>
    /// <returns>Livro deletado</returns>
    [HttpDelete("DeletarLivros/{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<LivrosDTOResponse>> DeleteAsync(int id)
    {
        var deletarLivro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);
        if(deletarLivro == null) return NotFound("Livro não localizado! Verifique o ID digitado");

        var livroExcluido = _uof.LivrosRepositorio.Delete(deletarLivro);
        await _uof.CommitAsync();

        var livroExcluidoDTO = _mapper.Map<LivrosDTOResponse>(livroExcluido);
        return Ok(livroExcluidoDTO);
    }
    #endregion

    #region METHODS
    private ActionResult<IEnumerable<Livros>> ObterLivros(IPagedList<Livros?> livros)
    {
        var metadados = new
        {
            livros.Count,
            livros.PageSize,
            livros.PageCount,
            livros.TotalItemCount,
            livros.HasNextPage,
            livros.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadados));

        var livrosDTO = _mapper.Map<IEnumerable<LivrosDTOResponse>>(livros);
        return Ok(livrosDTO);
    }
    #endregion
}
