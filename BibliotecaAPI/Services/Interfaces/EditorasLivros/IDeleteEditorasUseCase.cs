using BibliotecaAPI.DTOs.EditoraDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IDeleteEditorasUseCase
{
    Task<EditorasDTOResponse> DeleteAsync(int id);
}
