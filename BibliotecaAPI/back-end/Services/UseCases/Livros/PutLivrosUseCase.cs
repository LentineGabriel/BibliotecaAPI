using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Livros;

namespace BibliotecaAPI.Services.UseCases.Livros;
public class PutLivrosUseCase : IPutLivrosUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public PutLivrosUseCase(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<LivrosDTOResponse> PutAsync(int id , LivrosDTORequest livrosDTO)
    {
        // carrega livro + autor + editora + categoria
        var livroBanco = await _uof.LivrosRepositorio.GetLivroCompletoAsync(id);
        if(livroBanco == null) throw new NullReferenceException($"Livro por ID = {id} não encontrado. Por favor, tente novamente!");
        _mapper.Map(livrosDTO , livroBanco);

        _mapper.Map(livrosDTO , livroBanco);
        _uof.LivrosRepositorio.Update(livroBanco);
        await _uof.CommitAsync();

        return _mapper.Map<LivrosDTOResponse>(livroBanco);
    }
}
