using AutoMapper;
using BibliotecaAPI.DTOs.AuthDTOs.Users;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.UsuariosFiltro;
using BibliotecaAPI.Services.Interfaces.Auth.UsersUC;
using BibliotecaAPI.Services.Interfaces.TokenJWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.Auth.UsersUC;

public class GetUsersUseCase : IGetUsersUseCase
{
    #region PROS/CTOR
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public GetUsersUseCase(UserManager<ApplicationUser> userManager , IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<UsersResponseDTO>> GetUsersAsync()
    {
        var usuarios = await _userManager.Users.ToListAsync();
        if(usuarios == null || !usuarios.Any()) throw new NullReferenceException("Usuários não encontrados. Por favor, tente novamente!");

        return _mapper.Map<IEnumerable<UsersResponseDTO>>(usuarios);
    }

    public async Task<UsersResponseDTO> GetUserByIdAsync(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if(usuario == null) throw new NullReferenceException($"Não foi possível encontrar o usuário com o ID {id}. Por favor, tente novamente!");

        return _mapper.Map<UsersResponseDTO>(usuario);
    }

    public async Task<IPagedList<UsersResponseDTO>> GetPaginationAsync(UsuariosParameters usuariosParameters)
    {
        var usuarios = await _userManager.Users.ToListAsync();
        if(usuarios == null || !usuarios.Any()) throw new NullReferenceException("Usuários não encontrados. Por favor, tente novamente!");

        var usuariosDto = _mapper.Map<IEnumerable<UsersResponseDTO>>(usuarios);
        return await usuariosDto.ToPagedListAsync(usuariosParameters.PageNumber , usuariosParameters.PageSize);
    }
}
