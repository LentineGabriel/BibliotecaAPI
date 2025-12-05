using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EditorasDTOResponse>>> GetAsync()
    {
        var editoras = await _uof.EditorasRepositorio.GetAllAsync();
        if(editoras == null || !editoras.Any()) return BadRequest("Editoras não encontradas. Por favor, tente novamente!");

        var editorasDTO = _mapper.Map<IEnumerable<EditorasDTOResponse>>(editoras);
        return Ok(editorasDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterIdEditora")]
    public async Task<ActionResult<EditorasDTOResponse>> GetByIdAsync(int id)
    {
        var editora = await _uof.EditorasRepositorio.GetIdAsync(e => e.IdEditora == id);
        if(editora == null) return NotFound($"Editora por ID = {id} não encontrada. Por favor, tente novamente!");
        
        var editoraDTO = _mapper.Map<EditorasDTOResponse>(editora);
        return Ok(editoraDTO);
    }
    #endregion

    #region POST
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
}
