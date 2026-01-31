using BibliotecaAPI.DTOs.AutorDTOs;

namespace BibliotecaAPI.Services.Interfaces.Autores;
public interface IDeleteAutoresUseCase
{
    Task<AutorDTOResponse> DeleteAsync(int id);
}
