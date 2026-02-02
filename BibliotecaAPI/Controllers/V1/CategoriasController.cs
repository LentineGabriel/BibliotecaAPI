#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.CategoriasFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Categorias;
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
public class CategoriasController : ControllerBase
{
    #region PROPS/CTORS
    private readonly IMapper _mapper;
    private readonly IGetCategoriasUseCase _getCategoriasUseCase;
    private readonly ICreateCategoriasUseCase _createCategoriasUseCase;
    private readonly IPutCategoriasUseCase _putCategoriasUseCase;
    private readonly IPatchCategoriasUseCase _patchCategoriasUseCase;
    private readonly IDeleteCategoriasUseCase _deleteCategoriasUseCase;

    public CategoriasController(IUnityOfWork uof, IMapper mapper, IGetCategoriasUseCase getCategoriasUseCase, ICreateCategoriasUseCase categoriasUseCase, IPutCategoriasUseCase putCategoriasUseCase, IPatchCategoriasUseCase patchCategoriasUseCase, IDeleteCategoriasUseCase deleteCategoriasUseCase)
    {
        _mapper = mapper;
        _getCategoriasUseCase = getCategoriasUseCase;
        _createCategoriasUseCase = categoriasUseCase;
        _putCategoriasUseCase = putCategoriasUseCase;
        _patchCategoriasUseCase = patchCategoriasUseCase;
        _deleteCategoriasUseCase = deleteCategoriasUseCase;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna todas as categorias de livros cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de categorias</returns>
    // GET: /Categorias
    [HttpGet]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<CategoriasDTOResponse>>> GetAsync()
    {
        var categorias = await _getCategoriasUseCase.GetAllAsync();
        return Ok(categorias);
    }

    /// <summary>
    /// Retorna uma categoria de livro específica pelo ID.
    /// </summary>
    /// <returns>Categoria de livros via ID</returns>
    // GET: /Categorias/{id}
    [HttpGet("{id:int:min(1)}", Name = "ObterIdCategoria")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> GetByIdAsync(int id)
    {
        var categoriaId = await _getCategoriasUseCase.GetByIdAsync(id);
        return Ok(categoriaId);
    }

    /// <summary>
    /// Retorna as categorias de livros cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Editoras paginadas</returns>
    // GET: /Categorias/Paginacao
    [HttpGet("Paginacao")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Categorias>>> GetPaginationAsync([FromQuery] CategoriasParameters categoriaParameters)
    {
        var categoriasPaginadas = await _getCategoriasUseCase.GetPaginationAsync(categoriaParameters);
        return ObterCategorias(categoriasPaginadas);
    }

    /// <summary>
    /// Retorna uma categoria de livros filtrando pelo nome (via paginação).
    /// </summary>
    /// <returns>Categorias por nome</returns>
    // GET: /Categorias/PesquisaPorNome
    [HttpGet("PesquisaPorNome")]
    [ApiVersion("1.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Categorias>>> GetFilterNamePaginationAsync([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
    {
        var categoriasFiltradasPorNome = await _getCategoriasUseCase.GetFilterNamePaginationAsync(categoriasFiltroNome);
        return ObterCategorias(categoriasFiltradasPorNome);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona uma nova categoria de livro ao sistema.
    /// </summary>
    /// <returns>Categoria criada</returns>
    [HttpPost("AdicionarCategorias")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> PostAsync([FromBody] CategoriasDTORequest categoriasDTO)
    {
        if(categoriasDTO == null) return BadRequest("Não foi possível adicionar uma nova categoria. Tente novamente mais tarde!");
        var categoriaNova = await _createCategoriasUseCase.PostAsync(categoriasDTO);
        
        return Ok(categoriaNova);
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza uma categoria de livro existente no sistema.
    /// </summary>
    /// <returns></returns>
    [HttpPut("AtualizarCategoria/{id:int:min(1)}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> PutAsync(int id , CategoriasDTORequest categoriasDTO)
    {
        if(id != categoriasDTO.IdCategoria) return BadRequest($"Não foi possível encontrar a categoria com ID {id}. Por favor, verifique o ID digitado e tente novamente!");
        var categoriaAtualizada = await _putCategoriasUseCase.PutAsync(categoriasDTO);

        return Ok(categoriaAtualizada);
    }
    #endregion

    #region PATCH
    /// <summary>
    /// Atualiza partes de uma categoria de livros existente no sistema.
    /// </summary>
    /// <returns>Categoria atualizada</returns>
    // PATCH: /Categorias/AtualizarParcialCategoria/{id}
    [HttpPatch("AtualizarParcialCategoria/{id:int:min(1)}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> PatchAsync(int id , JsonPatchDocument<CategoriasDTORequest> patchDoc)
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");
        var categoriaRetornoDTO = await _patchCategoriasUseCase.PatchAsync(id , patchDoc);

        return Ok(categoriaRetornoDTO);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Deleta uma categoria de livros existente no sistema.
    /// </summary>
    /// <returns>Categoria deletada</returns>
    // DELETE: /Categorias/DeletarCategoria/{id}
    [HttpDelete("DeletarCategoria/{id:int:min(1)}")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> DeleteAsync(int id)
    {
        var categoriaExcluidaDTO = await _deleteCategoriasUseCase.DeleteAsync(id);
        return Ok(categoriaExcluidaDTO);
    }
    #endregion

    #region METHODS
    private ActionResult<IEnumerable<Categorias>> ObterCategorias(IPagedList<Categorias> categorias)
    {
        var metadados = new
        {
            categorias.Count ,
            categorias.PageSize ,
            categorias.PageCount ,
            categorias.TotalItemCount ,
            categorias.HasNextPage ,
            categorias.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination" , JsonConvert.SerializeObject(metadados));

        var categoriaDTO = _mapper.Map<IEnumerable<CategoriasDTOResponse>>(categorias);
        return Ok(categoriaDTO);
    }
    #endregion
}
