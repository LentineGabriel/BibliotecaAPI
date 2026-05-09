using BibliotecaAPI.DTOs.EditoraDTOs;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface ICreateEditorasUseCase
{
    Task<EditorasDTOResponse> PostAsync(EditorasDTORequest editorasDTO);
}
