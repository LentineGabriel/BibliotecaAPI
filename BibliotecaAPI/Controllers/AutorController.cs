using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorDTOResponse>>> GetAsync()
    {
        var autores = await _uof.AutorRepositorio.GetAllAsync();
        if(autores == null || !autores.Any()) return NotFound("Autores não encontrados. Por favor, tente novamente!");

        var autoresDTO = _mapper.Map<IEnumerable<AutorDTOResponse>>(autores);
        return Ok(autoresDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterIdAutor")]
    public async Task<ActionResult<AutorDTOResponse>> GetByIdAsync(int id)
    {
        var autor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(autor == null) return NotFound($"Autor por ID = {id} não encontrado. Por favor, tente novamente!");
        
        var autorDTO = _mapper.Map<AutorDTOResponse>(autor);
        return Ok(autorDTO);
    }
    #endregion

    #region POST
    [HttpPost("AdicionarAutores")]
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
    [HttpPut("AtualizarAutor/{id:int:min(1)}")]
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
    [HttpPatch("AtualizarParcialAutor/{id:int:min(1)}")]
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
    [HttpDelete("DeletarAutor/{id:int:min(1)}")]
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
}
