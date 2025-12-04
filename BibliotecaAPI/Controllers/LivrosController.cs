using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LivrosController : ControllerBase
{
    #region PROPS/CTORS
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public LivrosController(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    #region GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LivrosDTO>>> GetAsync()
    {
        var livros = await _uof.LivrosRepositorio.GetLivroCompletoAsync();
        if(livros == null || !livros.Any()) return NotFound("Livros não encontrados. Por favor, tente novamente!");

        var livrosDTO = _mapper.Map<IEnumerable<LivrosDTO>>(livros);
        return Ok(livrosDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterIdLivro")]
    public async Task<ActionResult<LivrosDTO>> GetByIdAsync(int id)
    {
        var livro = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        if(livro == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");
        
        var livroDTO = _mapper.Map<LivrosDTO>(livro);
        return Ok(livroDTO);
    }
    #endregion

    #region POST
    [HttpPost("AdicionarLivro")]
    public async Task<ActionResult<LivrosDTO>> PostAsync(LivrosDTO livrosDTO)
    {
        if(livrosDTO == null) return BadRequest("Não foi possível adicionar um novo livro. Tente novamente mais tarde!");

        var livroNovo = _mapper.Map<Livros>(livrosDTO);
        var livroCriado = _uof.LivrosRepositorio.Create(livroNovo);
        await _uof.CommitAsync();

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(livroCriado.IdLivro);

        var livroDTO = _mapper.Map<LivrosDTO>(livroCompleto);
        return new CreatedAtRouteResult("ObterIdLivro" , new { id = livroDTO.IdLivro } , livroDTO);
    }
    #endregion

    #region PUT
    [HttpPut("AtualizarLivro/{id:int:min(1)}")]
    public async Task<ActionResult<LivrosDTO>> PutAsync(int id, LivrosDTO livrosDTO)
    {
        if(id != livrosDTO.IdLivro) return BadRequest($"Não foi possível encontrar o livro com ID {id}. Por favor, verifique o ID digitado e tente novamente!");

        var livro = _mapper.Map<Livros>(livrosDTO);
        var livroExistente = _uof.LivrosRepositorio.Update(livro);
        await _uof.CommitAsync();

        var livroDTO = _mapper.Map<LivrosDTO>(livroExistente);
        return Ok(livroDTO);
    }
    #endregion

    #region PATCH
    [HttpPatch("AtualizarParcialLivro/{id:int:min(1)}")]
    public async Task<ActionResult<LivrosDTO>> PatchAsync(int id , JsonPatchDocument<LivrosDTO> patchDoc) 
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");

        var livro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);
        if(livro == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");

        var livroDTO = _mapper.Map<LivrosDTO>(livro);
        patchDoc.ApplyTo(livroDTO , ModelState);
        if(!ModelState.IsValid) return BadRequest(ModelState);

        _mapper.Map(livroDTO , livro);
        _uof.LivrosRepositorio.Update(livro);
        await _uof.CommitAsync();

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        var livroCompletoDTO = _mapper.Map<LivrosDTO>(livroCompleto);

        return Ok(livroCompletoDTO);
    }
    #endregion

    #region DELETE
    [HttpDelete("DeletarLivros/{id:int:min(1)}")]
    public async Task<ActionResult<LivrosDTO>> DeleteAsync(int id)
    {
        var deletarLivro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);

        if(deletarLivro == null) return NotFound("Livro não localizado! Verifique o ID digitado");

        var livroExcluido = _uof.LivrosRepositorio.Delete(deletarLivro);
        await _uof.CommitAsync();

        var livroExcluidoDTO = _mapper.Map<LivrosDTO>(livroExcluido);
        return Ok(livroExcluidoDTO);
    }
    #endregion
}
