using BibliotecaAPI.DTOs.CategoriaDTOs;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface ICreateCategoriasUseCase
{
    Task<CategoriasDTOResponse> PostAsync(CategoriasDTORequest categoriasDTO);
}
