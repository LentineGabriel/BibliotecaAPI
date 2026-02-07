using BibliotecaAPI.DTOs.LivrosDTOs;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface ICreateLivrosUseCase
{
    Task<LivrosDTOResponse> PostAsync(LivrosDTORequest livrosDTO);
}
