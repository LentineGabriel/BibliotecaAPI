using AutoMapper;
using BibliotecaAPI.DTOs.EditoraDTOs;
using BibliotecaAPI.Repositories.Interfaces;

namespace BibliotecaAPI.Services.Interfaces.EditorasLivros;
public interface IPutEditorasUseCase
{
    Task<EditorasDTOResponse> PutAsync(EditorasDTORequest editorasDTO);
}
