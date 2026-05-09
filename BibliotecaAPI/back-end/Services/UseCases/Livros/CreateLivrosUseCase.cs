using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Livros;

namespace BibliotecaAPI.Services.UseCases.Livros;

public class CreateLivrosUseCase : ICreateLivrosUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public CreateLivrosUseCase(IUnityOfWork uof , IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<LivrosDTOResponse> PostAsync(LivrosDTORequest livrosDTO)
    {
        var livroNovo = _mapper.Map<Models.Livros>(livrosDTO);
        var livroCriado = _uof.LivrosRepositorio.Create(livroNovo);
        await _uof.CommitAsync();

        var livroCompleto = await _uof.LivrosRepositorio.GetLivroCompletoAsync(livroCriado.IdLivro);
        return _mapper.Map<LivrosDTOResponse>(livroCompleto);
    }
}
