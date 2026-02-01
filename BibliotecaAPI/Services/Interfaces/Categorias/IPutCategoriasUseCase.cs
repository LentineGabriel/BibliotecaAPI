using BibliotecaAPI.DTOs.CategoriaDTOs;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface IPutCategoriasUseCase
{
    Task<CategoriasDTOResponse> PutAsync(CategoriasDTORequest categoriasDTO);
}
