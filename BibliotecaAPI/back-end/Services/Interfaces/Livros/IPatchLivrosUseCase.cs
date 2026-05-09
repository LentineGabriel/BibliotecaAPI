using BibliotecaAPI.DTOs.LivrosDTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface IPatchLivrosUseCase
{
    Task<LivrosDTOResponse> PatchAsync(int id , JsonPatchDocument<LivrosDTORequest> patchDoc);
    Task<IActionResult> PatchCategoriasAsync(int id , LivrosCategoriasPatchDTO dto);
}
