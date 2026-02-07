using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Livros;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.UseCases.Livros;

public class PatchLivrosUseCase : IPatchLivrosUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PatchLivrosUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<LivrosDTOResponse> PatchAsync(int id , JsonPatchDocument<LivrosDTORequest> patchDoc)
    {
        if(patchDoc == null) throw new NullReferenceException("Nenhuma opção foi enviada para atualizar parcialmente.");

        var livro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);
        if(livro == null) throw new NullReferenceException($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");

        var livroDTO = _mapper.Map<LivrosDTORequest>(livro);
        patchDoc.ApplyTo(livroDTO);

        _mapper.Map(livroDTO , livro);
        _uof.LivrosRepositorio.Update(livro);
        await _uof.CommitAsync();

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        return _mapper.Map<LivrosDTOResponse>(livroCompleto);
    }

    public async Task<IActionResult> PatchCategoriasAsync(int id , LivrosCategoriasPatchDTO dto)
    {
        var livro = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        if(livro == null) throw new NullReferenceException($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");

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

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        return new JsonResult(_mapper.Map<LivrosDTOResponse>(livroCompleto));
    }
}
