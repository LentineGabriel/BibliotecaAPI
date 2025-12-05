using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
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
    public async Task<ActionResult<IEnumerable<LivrosDTOResponse>>> GetAsync()
    {
        var livros = await _uof.LivrosRepositorio.GetLivroCompletoAsync();
        if(livros == null || !livros.Any()) return NotFound("Livros não encontrados. Por favor, tente novamente!");

        var livrosDTO = _mapper.Map<IEnumerable<LivrosDTOResponse>>(livros);
        return Ok(livrosDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterIdLivro")]
    public async Task<ActionResult<LivrosDTOResponse>> GetByIdAsync(int id)
    {
        var livro = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        if(livro == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");
        
        var livroDTO = _mapper.Map<LivrosDTOResponse>(livro);
        return Ok(livroDTO);
    }
    #endregion

    #region POST
    [HttpPost("AdicionarLivro")]
    public async Task<ActionResult<LivrosDTOResponse>> PostAsync(LivrosDTORequest livrosDTO)
    {
        if(livrosDTO == null) return BadRequest("Não foi possível adicionar um novo livro. Tente novamente mais tarde!");

        var livroNovo = _mapper.Map<Livros>(livrosDTO);
        var livroCriado = _uof.LivrosRepositorio.Create(livroNovo);
        await _uof.CommitAsync();

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(livroCriado.IdLivro);

        var livroDTO = _mapper.Map<LivrosDTOResponse>(livroCompleto);
        return new CreatedAtRouteResult("ObterIdLivro" , new { id = livroCriado.IdLivro } , livroDTO);
    }
    #endregion

    #region PUT
    [HttpPut("AtualizarLivro/{id:int:min(1)}")]
    public async Task<ActionResult<LivrosDTOResponse>> PutAsync(int id, LivrosDTORequest livrosDTO)
    {
        if(id != livrosDTO.IdLivro) return BadRequest($"Não foi possível encontrar o livro com ID {id}. Por favor, verifique o ID digitado e tente novamente!");

        // carrega livro + autor + editora + categoria
        var livroBanco = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        if(livroBanco == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");
        _mapper.Map(livrosDTO , livroBanco);

        _mapper.Map(livrosDTO , livroBanco);
        _uof.LivrosRepositorio.Update(livroBanco);
        await _uof.CommitAsync();

        var livroDTO = _mapper.Map<LivrosDTOResponse>(livroBanco);
        return Ok(livroDTO);
    }
    #endregion

    #region PATCH
    [HttpPatch("AtualizarParcialLivro/{id:int:min(1)}")]
    public async Task<ActionResult<LivrosDTOResponse>> PatchAsync(int id , JsonPatchDocument<LivrosDTORequest> patchDoc) 
    {
        if(patchDoc == null) return BadRequest("Nenhuma opção foi enviada para atualizar parcialmente.");

        var livro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);
        if(livro == null) return NotFound($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");

        var livroDTO = _mapper.Map<LivrosDTORequest>(livro);
        patchDoc.ApplyTo(livroDTO , ModelState);
        if(!ModelState.IsValid) return BadRequest(ModelState);

        _mapper.Map(livroDTO , livro);
        _uof.LivrosRepositorio.Update(livro);
        await _uof.CommitAsync();

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        var livroCompletoDTO = _mapper.Map<LivrosDTOResponse>(livroCompleto);

        return Ok(livroCompletoDTO);
    }
    #endregion

    #region DELETE
    [HttpDelete("DeletarLivros/{id:int:min(1)}")]
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
}
