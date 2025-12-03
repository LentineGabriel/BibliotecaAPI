using BibliotecaAPI.Context;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repositories.Interfaces;

namespace BibliotecaAPI.Repositories;

public class EditorasRepositorio : Repositorio<Editoras>, IEditorasRepositorio
{
    public EditorasRepositorio(AppDbContext context) : base(context) { }
}
