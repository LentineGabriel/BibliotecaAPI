using BibliotecaAPI.DTOs.EditoraDTOs;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IPutEditorasUseCase
{
    Task<EditorasDTOResponse> PutAsync(EditorasDTORequest editorasDTO);
}
