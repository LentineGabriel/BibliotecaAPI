using AutoMapper;
using BibliotecaAPI.DTOs.AutorDTOs;
using BibliotecaAPI.Models;
using BibliotecaAPI.Pagination.AutoresFiltro;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Autores;
using X.PagedList;

namespace BibliotecaAPI.Services.UseCases.Autores;
public class GetAutoresUseCase : IGetAutoresUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public GetAutoresUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<IEnumerable<AutorDTOResponse>> GetAllAsync()
    {
        var autores = await _uof.AutorRepositorio.GetAllAsync();
        if(autores == null || !autores.Any()) throw new ArgumentNullException("Autores não encontrados. Por favor, tente novamente!");

        return _mapper.Map<IEnumerable<AutorDTOResponse>>(autores);
    }
    public async Task<AutorDTOResponse> GetByIdAsync(int id)
    {
        var autor = await _uof.AutorRepositorio.GetIdAsync(a => a.IdAutor == id);
        if(autor == null) throw new KeyNotFoundException($"Autor por ID = {id} não encontrado. Por favor, tente novamente!");

        return _mapper.Map<AutorDTOResponse>(autor);
    }
    public async Task<IPagedList<Autor>> GetPaginationAsync(AutoresParameters autoresParameters) => await _uof.AutorRepositorio.GetAutoresAsync(autoresParameters);
    public async Task<IPagedList<Autor>> GetFilterNamePaginationAsync(AutoresFiltroNome autoresFiltroNome) => await _uof.AutorRepositorio.GetAutoresFiltrandoPeloNome(autoresFiltroNome);
    public async Task<IPagedList<Autor>> GetFilterNationalityPaginationAsync(AutoresFiltroNacionalidade autoresFiltroNacionalidade) => await _uof.AutorRepositorio.GetAutoresFiltrandoPelaNacionalidade(autoresFiltroNacionalidade);
}
