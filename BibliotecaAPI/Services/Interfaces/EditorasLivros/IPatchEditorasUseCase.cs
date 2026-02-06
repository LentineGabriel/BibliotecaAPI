using BibliotecaAPI.DTOs.EditoraDTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IPatchEditorasUseCase
{
    Task<EditorasDTOResponse> PatchAsync(JsonPatchDocument<EditorasDTORequest> patchDoc);
}
