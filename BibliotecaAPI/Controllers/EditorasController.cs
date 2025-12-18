using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.EditorasFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EditorasController : ControllerBase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public EditorasController(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    #region GET
    /// <summary>
    /// Retorna todas as editoras cadastradas no sistema.
    /// </summary>
    /// <returns>Lista de editoras</returns>
    // GET: /Editoras
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EditorasDTOResponse>>> GetAsync()
    {
        var editoras = await _uof.EditorasRepositorio.GetAllAsync();
        if(editoras == null || !editoras.Any()) return BadRequest("Editoras não encontradas. Por favor, tente novamente!");

        var editorasDTO = _mapper.Map<IEnumerable<EditorasDTOResponse>>(editoras);
        return Ok(editorasDTO);
    }

    /// <summary>
    /// Retorna uma editora específica pelo ID.
    /// </summary>
    /// <returns>Editora via ID</returns>
    // GET: /Editoras/{id}
    [HttpGet("{id:int:min(1)}", Name = "ObterIdEditora")]
    public async Task<ActionResult<EditorasDTOResponse>> GetByIdAsync(int id)
    {
        var editora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        if(editora == null) return NotFound($"Editora por ID = {id} não encontrada. Por favor, tente novamente!");
        
        var editoraDTO = _mapper.Map<EditorasDTOResponse>(editora);
        return Ok(editoraDTO);
    }

    /// <summary>
    /// Retorna as editoras cadastrados no sistema via paginação.
    /// </summary>
    /// <returns>Lista de Editoras paginadas</returns>
    // GET: /Editoras/Paginacao
    [HttpGet("Paginacao")]
    public async Task<ActionResult<IEnumerable<Editoras>>> GetPaginationAsync([FromQuery] EditorasParameters editorasParameters)
    {
        var editoras = await _uof.EditorasRepositorio.GetEditorasAsync(editorasParameters);
        return ObterEditoras(editoras);
    }

    /// <summary>
    /// Retorna as editoras filtrando pelo nome (via paginação).
    /// </summary>
    /// <returns>Editoras por nome</returns>
    // GET: /Editoras/PesquisaPorNome
    [HttpGet("PesquisaPorNome")]
    public async Task<ActionResult<IEnumerable<Editoras>>> GetFilterNamePaginationAsync([FromQuery] EditorasFiltroNome editorasFiltroNome)
    {
        var editoras = await _uof.EditorasRepositorio.GetEditorasFiltrandoPeloNome(editorasFiltroNome);
        return ObterEditoras(editoras);
    }

    /// <summary>
    /// Retorna as editoras filtrando pelo país de origem (via paginação).
    /// </summary>
    /// <returns>Editoras por nacionalidade</returns>
    // GET: /Editoras/PesquisaPorPaisDeOrigem
    [HttpGet("PesquisaPorNacionalidade")]
    public async Task<ActionResult<IEnumerable<Editoras>>> GetFilterNationalityPaginationAsync([FromQuery] EditorasFiltroPaisOrigem editorasFiltroPaisOrigem)
    {
        var editoras = await _uof.EditorasRepositorio.GetEditorasFiltrandoPorPaisDeOrigem(editorasFiltroPaisOrigem);
        return ObterEditoras(editoras);
    }
    #endregion

    #region POST
    /// <summary>
    /// Adiciona uma nova editora ao sistema.
    /// </summary>
    /// <returns>Categoria criada</returns>
    [HttpPost("AdicionarEditoras")]
    public async Task<ActionResult<EditorasDTOResponse>> PostAsync(EditorasDTORequest editorasDTO)
    {
        if(editorasDTO == null) return BadRequest("Não foi possível adicionar uma nova editora. Tente novamente mais tarde!");

        var editoraNova = _mapper.Map<Editoras>(editorasDTO);
        var editoraCriada = _uof.EditorasRepositorio.Create(editoraNova);
        await _uof.CommitAsync();
        
        var editoraRetornoDTO = _mapper.Map<EditorasDTOResponse>(editoraCriada);
        return new CreatedAtRouteResult("ObterIdEditora" , new { id = editoraCriada.IdEditora } , editoraRetornoDTO);
    }
    #endregion

    #region PUT
    /// <summary>
    /// Atualiza uma editora existente no sistema.
    /// </summary>
    /// <returns></returns>
    [HttpPut("AtualizarEditora/{id:int:min(1)}")]
    public async Task<ActionResult<EditorasDTOResponse>> PutAsync(int id , EditorasDTORequest editorasDTO)
    {
        if(id != editorasDTO.IdEditora) return BadRequest($"Não foi possível encontrar a editora com ID {id}. Por favor, verifique o ID e tente novamente!");
        
        var editora = _mapper.Map<Editoras>(editorasDTO);
        var editoraExistente = _uof.EditorasRepositorio.Update(editora);
        await _uof.CommitAsync();

        var editoraRetornoDTO = _mapper.Map<EditorasDTOResponse>(editoraExistente);
        return Ok(editoraRetornoDTO);
    }
    #endregion

    #region PATCH
    /// <summary>
    /// Atualiza partes de uma editora existente no sistema.
    /// </summary>
    /// <returns>Editora atualizada</returns>
    [HttpPatch("AtualizarParcialEditora/{id:int:min(1)}")]
    public async Task<ActionResult<EditorasDTOResponse>> PatchAsync(int id , JsonPatchDocument<EditorasDTORequest> patchDoc)
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");

        var editora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        if(editora == null) return NotFound($"Editora por ID = {id} não encontrada. Por favor, tente novamente!");

        var editoraDTO = _mapper.Map<EditorasDTORequest>(editora);
        patchDoc.ApplyTo(editoraDTO , ModelState);
        if(!ModelState.IsValid) return BadRequest(ModelState);

        _mapper.Map(editoraDTO , editora);
        _uof.EditorasRepositorio.Update(editora);
        await _uof.CommitAsync();

        var autorRetornoDTO = _mapper.Map<EditorasDTOResponse>(editora);
        return Ok(editoraDTO);
    }
    #endregion

    #region DELETE
    /// <summary>
    /// Deleta uma editora existente no sistema.
    /// </summary>
    /// <returns>Editora deletada</returns>
    [HttpDelete("DeletarEditora/{id:int:min(1)}")]
    public async Task<ActionResult<EditorasDTOResponse>> DeleteAsync(int id)
    {
        var deletarEditora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        if(deletarEditora == null) return NotFound($"Editora não localizada! Verifique o ID digitado");
        
        var editoraExcluida = _uof.EditorasRepositorio.Delete(deletarEditora);
        await _uof.CommitAsync();
        
        var editoraExcluidaDTO = _mapper.Map<EditorasDTOResponse>(editoraExcluida);
        return Ok(editoraExcluidaDTO);
    }
    #endregion

    #region METHODS
    private ActionResult<IEnumerable<Editoras>> ObterEditoras(IPagedList<Editoras> editoras)
    {
        var metadados = new
        {
            editoras.Count ,
            editoras.PageSize ,
            editoras.PageCount ,
            editoras.TotalItemCount ,
            editoras.HasNextPage ,
            editoras.HasPreviousPage
        };
        Response.Headers.Append("X-Pagination" , JsonConvert.SerializeObject(metadados));

        var editorasDTO = _mapper.Map<IEnumerable<EditorasDTOResponse>>(editoras);
        return Ok(editorasDTO);
    }
    #endregion
}
