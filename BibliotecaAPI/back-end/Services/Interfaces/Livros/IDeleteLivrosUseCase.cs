using BibliotecaAPI.DTOs.LivrosDTOs;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface IDeleteLivrosUseCase
{
    Task<LivrosDTOResponse> DeleteAsync(int id);
}
