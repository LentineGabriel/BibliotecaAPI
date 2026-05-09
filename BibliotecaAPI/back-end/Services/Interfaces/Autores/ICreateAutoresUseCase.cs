using BibliotecaAPI.DTOs.AutorDTOs;

namespace BibliotecaAPI.Services.Interfaces.Autores;
public interface ICreateAutoresUseCase
{
    Task<AutorDTOResponse> CreateAsync(AutorDTORequest autorDTO);
}
