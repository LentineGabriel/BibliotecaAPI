using BibliotecaAPI.DTOs.AutorDTOs;

namespace BibliotecaAPI.Services.Interfaces.Autores;
public interface IPutAutoresUseCase
{
    Task<AutorDTOResponse> PutAsync(AutorDTORequest autorDTO);
}
