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

    public async Task<IEnumerable<UsersDTOResponse>> GetUsersAsync()
    {
        var usuarios = await _userManager.Users.ToListAsync();
        if(usuarios == null || !usuarios.Any()) throw new NullReferenceException("Usuários não encontrados. Por favor, tente novamente!");

        return _mapper.Map<IEnumerable<UsersDTOResponse>>(usuarios);
    }

    public async Task<UsersDTOResponse> GetUserByIdAsync(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if(usuario == null) throw new NullReferenceException($"Não foi possível encontrar o usuário com o ID {id}. Por favor, tente novamente!");

        return _mapper.Map<UsersDTOResponse>(usuario);
    }

    public async Task<IPagedList<UsersDTOResponse>> GetPaginationAsync(UsuariosParameters usuariosParameters)
    {
        var usuarios = await _userManager.Users.ToListAsync();
        if(usuarios == null || !usuarios.Any()) throw new NullReferenceException("Usuários não encontrados. Por favor, tente novamente!");

        var usuariosDto = _mapper.Map<IEnumerable<UsersDTOResponse>>(usuarios);
        return await usuariosDto.ToPagedListAsync(usuariosParameters.PageNumber , usuariosParameters.PageSize);
    }

    public Task<IPagedList<UsersDTOResponse>> GetUserEstanteAsync(string id)
    {
        throw new NotImplementedException();
    }
}
