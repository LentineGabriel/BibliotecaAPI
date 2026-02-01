using BibliotecaAPI.DTOs.CategoriaDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Categorias;
public interface ICreateCategoriasUseCase
{
    Task<CategoriasDTOResponse> PostAsync(CategoriasDTORequest categoriasDTO);
}
