using BibliotecaAPI.Context;
using BibliotecaAPI.Enums;
using BibliotecaAPI.Repositories.Interfaces;
using BibliotecaAPI.Services.Interfaces.EstanteUC;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Services.UseCases.EstanteUC;

public class CreateLivroEstante : ICreateLivroEstante
{
    #region PROPS/CTOR
    private readonly IEstanteRepositorio _estanteRepositorio;
    private readonly AppDbContext _context;

    public CreateLivroEstante(IEstanteRepositorio estanteRepositorio , AppDbContext context)
    {
        _estanteRepositorio = estanteRepositorio;
        _context = context;
    }
    #endregion

    public async Task<Estante> CreateAsync(string userId , int livroId)
    {
        // verificar se o livro existe
        var livro = await _context.Livros.AnyAsync(l => l.IdLivro == livroId);
        if(!livro) throw new Exception("Livro não encontrado.");

        // verificar duplicidade
        var existe = await _estanteRepositorio.GetByUserAndLivroAsync(userId, livroId);
        if(existe != null) throw new Exception("Livro já adicionado à estante.");

        // criar entidade
        var estante = new Estante
        {
            UserId = userId ,
            LivroId = livroId ,
            StatusLeitura = StatusLivroEstante.QueroLer
        };

        await _estanteRepositorio.AddAsync(estante);
        await _estanteRepositorio.SaveChangesAsync();

        return estante;
    }
}
