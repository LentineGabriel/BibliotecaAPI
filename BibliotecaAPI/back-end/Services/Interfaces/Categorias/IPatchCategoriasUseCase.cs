using BibliotecaAPI.DTOs.CategoriaDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface IPatchCategoriasUseCase
{
    Task<CategoriasDTOResponse> PatchAsync(int id , JsonPatchDocument<CategoriasDTORequest> patchDoc);
}
