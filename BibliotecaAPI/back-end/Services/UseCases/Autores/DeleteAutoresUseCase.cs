using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Autores;

namespace BibliotecaAPI.Services.UseCases.Autores;
public class DeleteAutoresUseCase : IDeleteAutoresUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public DeleteAutoresUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<AutorDTOResponse> DeleteAsync(int id)
    {
        var deletarAutor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(deletarAutor == null) throw new KeyNotFoundException("Autor não localizado! Verifique o ID digitado");

        var autorExcluido = _uof.AutorRepositorio.Delete(deletarAutor);
        await _uof.CommitAsync();

        return _mapper.Map<AutorDTOResponse>(autorExcluido);
    }
}
