using BibliotecaAPI.DTOs.CategoriaDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface IDeleteCategoriasUseCase
{
    Task<CategoriasDTOResponse> DeleteAsync(int id);
}
