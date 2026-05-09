using AutoMapper;
using BibliotecaAPI.DTOs.LivrosDTOs;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.Livros;

namespace BibliotecaAPI.Services.UseCases.Livros;
public class DeleteLivrosUseCase :IDeleteLivrosUseCase
{
    #region PROPS/CTOR
    private readonly IUnityOfWork _uof;
    private readonly IMapper _mapper;

    public DeleteLivrosUseCase(IUnityOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }
    #endregion

    public async Task<LivrosDTOResponse> DeleteAsync(int id)
    {
        var deletarLivro = await _uof.LivrosRepositorio.GetIdAsync(l => l.IdLivro == id);
        if(deletarLivro == null) throw new NullReferenceException("Livro não localizado! Verifique o ID digitado");

        var livroExcluido = _uof.LivrosRepositorio.Delete(deletarLivro);
        await _uof.CommitAsync();

        return _mapper.Map<LivrosDTOResponse>(livroExcluido);
    }
}
