using BibliotecaAPI.DTOs.LivrosDTOs;

namespace BibliotecaAPI.Services.Interfaces.Livros;
public interface IPutLivrosUseCase
{
    Task<LivrosDTOResponse> PutAsync(int id , LivrosDTORequest livrosDTO);
}
