using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
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
        var livros = await _uof.LivrosRepositorio.GetAllAsync();
        if(livros == null) return NotFound("Livros não encontrados. Por favor, tente novamente!");

        var livrosDTO = _mapper.Map<IEnumerable<LivrosDTO>>(livros);
        return Ok(livrosDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterId")]
    public async Task<ActionResult<LivrosDTO>> GetByIdAsync(int id)
    {
        var livro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);
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

        var livroDTO = _mapper.Map<LivrosDTO>(livroCriado);
        return new CreatedAtRouteResult("ObterId" , new { id = livroDTO.IdLivro } , livroDTO);
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

    #region DELETE
    [HttpDelete]
    public async Task<ActionResult<LivrosDTO>> DeleteAsync(int id)
    {
        var deletarLivro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);

        if(deletarLivro == null) return NotFound("Livro não localizada! Verifique o ID digitado");

        var livroExcluido = _uof.LivrosRepositorio.Delete(deletarLivro);
        await _uof.CommitAsync();

        var categoriaExcluidaDTO = _mapper.Map<LivrosDTO>(livroExcluido);
        return Ok(categoriaExcluidaDTO);
    }
    #endregion
}
