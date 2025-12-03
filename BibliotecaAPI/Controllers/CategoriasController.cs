using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriasDTO>>> GetAsync()
    {
        var categorias = await _uof.CategoriaLivrosRepositorio.GetAllAsync();
        if(categorias == null) return NotFound("Categorias não encontrados. Por favor, tente novamente!");

        var categoriasDTO = _mapper.Map<IEnumerable<CategoriasDTO>>(categorias);
        return Ok(categoriasDTO);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterIdCategoria")]
    public async Task<ActionResult<CategoriasDTO>> GetByIdAsync(int id)
    {
        var categoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);
        if(categoria == null) return NotFound($"Categoria por ID = {id} não encontrado. Por favor, tente novamente!");
        
        var categoriaDTO = _mapper.Map<CategoriasDTO>(categoria);
        return Ok(categoriaDTO);
    }
    #endregion

    #region POST
    [HttpPost("AdicionarCategorias")]
    public async Task<ActionResult<CategoriasDTO>> PostAsync(CategoriasDTO categoriasDTO)
    {
        if(categoriasDTO == null) return BadRequest("Não foi possível adicionar uma nova categoria. Tente novamente mais tarde!");

        var categoriaNova = _mapper.Map<Categorias>(categoriasDTO);
        var categoriaCriada = _uof.CategoriaLivrosRepositorio.Create(categoriaNova);
        await _uof.CommitAsync();

        var categoriaDTO = _mapper.Map<CategoriasDTO>(categoriaCriada);
        return new CreatedAtRouteResult("ObterIdCategoria" , new { id = categoriaDTO.IdCategoria } , categoriaDTO);
    }
    #endregion

    #region PUT
    [HttpPut("AtualizarCategoria/{id:int:min(1)}")]
    public async Task<ActionResult<CategoriasDTO>> PutAsync(int id , CategoriasDTO categoriasDTO)
    {
        if(id != categoriasDTO.IdCategoria) return BadRequest($"Não foi possível encontrar a categoria com ID {id}. Por favor, verifique o ID digitado e tente novamente!");

        var categoria = _mapper.Map<Categorias>(categoriasDTO);
        var categoriaExistente = _uof.CategoriaLivrosRepositorio.Update(categoria);
        await _uof.CommitAsync();

        var categoriaDTO = _mapper.Map<CategoriasDTO>(categoriaExistente);
        return Ok(categoriaDTO);
    }
    #endregion

    #region DELETE
    [HttpDelete("DeletarCategoria/{id:int:min(1)}")]
    public async Task<ActionResult<CategoriasDTO>> DeleteAsync(int id)
    {
        var deletarCategoria = await _uof.CategoriaLivrosRepositorio.GetIdAsync(c => c.IdCategoria == id);

        if(deletarCategoria == null) return NotFound("Categoria não localizada! Verifique o ID digitado");

        var categoriaExcluida = _uof.CategoriaLivrosRepositorio.Delete(deletarCategoria);
        await _uof.CommitAsync();

        var categoriaExcluidaDTO = _mapper.Map<CategoriasDTO>(categoriaExcluida);
        return Ok(categoriaExcluidaDTO);
    }
    #endregion
}
