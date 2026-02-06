using BibliotecaAPI.DTOs.EditoraDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IPatchEditorasUseCase
{
    Task<EditorasDTOResponse> PatchAsync(int id , JsonPatchDocument<EditorasDTORequest> patchDoc);
}
