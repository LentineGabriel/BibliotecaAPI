using BibliotecaAPI.DTOs.AutorDTOs;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Services.Interfaces.Autores;
public interface ICreateAutoresUseCase
{
    Task<AutorDTOResponse> CreateAsync(AutorDTORequest autorDTO);
}
