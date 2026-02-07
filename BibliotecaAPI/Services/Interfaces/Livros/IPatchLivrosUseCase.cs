using BibliotecaAPI.DTOs.LivrosDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface IPatchLivrosUseCase
{
    Task<LivrosDTOResponse> PatchAsync(int id , JsonPatchDocument<LivrosDTORequest> patchDoc);
}
