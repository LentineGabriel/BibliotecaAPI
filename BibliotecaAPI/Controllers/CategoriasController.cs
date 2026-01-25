#region USINGS
using Asp.Versioning;
using AutoMapper;
using BibliotecaAPI.DTOs.CategoriaDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.CategoriasFiltro;
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
public class CategoriasController : ControllerBase
{
    #region PROPS/CTORS
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public CategoriasController(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
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
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<CategoriasDTOResponse>>> GetAsync()
    {
        var categorias = await _uof.CategoriaLivrosRepositorio.GetAllAsync();
        if(categorias == null || !categorias.Any()) return NotFound("Categorias não encontradas. Por favor, tente novamente!");

        var categoriasDTO = _mapper.Map<IEnumerable<CategoriasDTOResponse>>(categorias);
        return Ok(categoriasDTO);
    }

    /// <summary>
    /// Retorna uma categoria de livro específica pelo ID.
    /// </summary>
    /// <returns>Categoria de livros via ID</returns>
    // GET: /Categorias/{id}
    [HttpGet("{id:int:min(1)}", Name = "ObterIdCategoria")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> GetByIdAsync(int id)
    {
        var categoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(categoria == null) return NotFound($"Categoria por ID = {id} não encontrado. Por favor, tente novamente!");
        
        var categoriaDTO = _mapper.Map<CategoriasDTOResponse>(categoria);
        return Ok(categoriaDTO);
    }

    /// <summary>
    /// Retorna as categorias de livros cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Editoras paginadas</returns>
    // GET: /Categorias/Paginacao
    [HttpGet("Paginacao")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Categorias>>> GetPaginationAsync([FromQuery] CategoriasParameters categoriaParameters)
    {
        var categorias = await _uof.CategoriaLivrosRepositorio.GetCategoriasAsync(categoriaParameters);
        return ObterCategorias(categorias);
    }

    /// <summary>
    /// Retorna uma categoria de livros filtrando pelo nome (via paginação).
    /// </summary>
    /// <returns>Categorias por nome</returns>
    // GET: /Categorias/PesquisaPorNome
    [HttpGet("PesquisaPorNome")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsAndUsers")]
    public async Task<ActionResult<IEnumerable<Categorias>>> GetFilterNamePaginationAsync([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
    {
        var categorias = await _uof.CategoriaLivrosRepositorio.GetCategoriasFiltrandoPeloNome(categoriasFiltroNome);
        return ObterCategorias(categorias);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona uma nova categoria de livro ao sistema.
    /// </summary>
    /// <returns>Categoria criada</returns>
    [HttpPost("AdicionarCategorias")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> PostAsync(CategoriasDTORequest categoriasDTO)
    {
        if(categoriasDTO == null) return BadRequest("Não foi possível adicionar uma nova categoria. Tente novamente mais tarde!");

        var categoriaNova = _mapper.Map<Categorias>(categoriasDTO);
        var categoriaCriada = _uof.CategoriaLivrosRepositorio.Create(categoriaNova);
        await _uof.CommitAsync();

        var categoriaDTO = _mapper.Map<CategoriasDTOResponse>(categoriaCriada);
        return new CreatedAtRouteResult("ObterIdCategoria" , new { id = categoriaCriada.IdCategoria } , categoriaDTO);
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza uma categoria de livro existente no sistema.
    /// </summary>
    /// <returns></returns>
    [HttpPut("AtualizarCategoria/{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> PutAsync(int id , CategoriasDTORequest categoriasDTO)
    {
        if(id != categoriasDTO.IdCategoria) return BadRequest($"Não foi possível encontrar a categoria com ID {id}. Por favor, verifique o ID digitado e tente novamente!");

        var categoria = _mapper.Map<Categorias>(categoriasDTO);
        var categoriaExistente = _uof.CategoriaLivrosRepositorio.Update(categoria);
        await _uof.CommitAsync();

        var categoriaRetornoDTO = _mapper.Map<CategoriasDTOResponse>(categoriaExistente);
        return Ok(categoriaRetornoDTO);
    }
    #endregion

    #region PATCH
    /// <summary>
    /// Atualiza partes de uma categoria de livros existente no sistema.
    /// </summary>
    /// <returns>Categoria atualizada</returns>
    // PATCH: /Categorias/AtualizarParcialCategoria/{id}
    [HttpPatch("AtualizarParcialCategoria/{id:int:min(1)}")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> PatchAsync(int id , JsonPatchDocument<CategoriasDTORequest> patchDoc)
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");

        var categoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(categoria == null) return NotFound($"Categoria com ID {id} não encontrada. Por favor, verifique o ID digitado e tente novamente!");

        var categoriaDTO = _mapper.Map<CategoriasDTORequest>(categoria);
        patchDoc.ApplyTo(categoriaDTO , ModelState);
        if(!ModelState.IsValid) return BadRequest(ModelState);

        _mapper.Map(categoriaDTO , categoria);
        _uof.CategoriaLivrosRepositorio.Update(categoria);
        await _uof.CommitAsync();

        var categoriaRetornoDTO = _mapper.Map<CategoriasDTOResponse>(categoria);
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
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Authorize(Policy = "AdminsOnly")]
    public async Task<ActionResult<CategoriasDTOResponse>> DeleteAsync(int id)
    {
        var deletarCategoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(deletarCategoria == null) return NotFound("Categoria não localizada! Verifique o ID digitado");

        var categoriaExcluida = _uof.CategoriaLivrosRepositorio.Delete(deletarCategoria);
        await _uof.CommitAsync();

        var categoriaExcluidaDTO = _mapper.Map<CategoriasDTOResponse>(categoriaExcluida);
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
