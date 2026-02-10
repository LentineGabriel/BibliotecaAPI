using BibliotecaAPI.DTOs.EditoraDTOs;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IDeleteEditorasUseCase
{
    Task<EditorasDTOResponse> DeleteAsync(int id);
}
