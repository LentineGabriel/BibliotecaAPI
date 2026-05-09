using BibliotecaAPI.DTOs.CategoriaDTOs;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface IDeleteCategoriasUseCase
{
    Task<CategoriasDTOResponse> DeleteAsync(int id);
}
