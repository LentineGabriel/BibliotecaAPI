using BibliotecaAPI.DTOs.AutorDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.Autores;
public interface IPatchAutoresUseCase
{
    Task<AutorDTOResponse> PatchAsync(int id , JsonPatchDocument<AutorDTORequest> patchDoc);
}
