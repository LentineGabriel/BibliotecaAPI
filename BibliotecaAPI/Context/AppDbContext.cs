#region USINGS
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BibliotecaAPI.Context;
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    #region PROPS/CTOR
    public DbSet<Livros> Livros { get; set; }
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Editoras> Editoras { get; set; }
    public DbSet<Categorias> Categorias { get; set; }
    public DbSet<LivroCategoria> LivrosCategorias { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da relação muitos-para-muitos entre Livros e Categorias
        modelBuilder.Entity<LivroCategoria>().HasKey(lc => new { lc.LivroId, lc.CategoriaId });
        modelBuilder.Entity<LivroCategoria>().HasOne(lc => lc.Livros).WithMany(l => l.LivrosCategorias).HasForeignKey(lc => lc.LivroId);
        modelBuilder.Entity<LivroCategoria>().HasOne(lc => lc.Categorias).WithMany(c => c.LivrosCategorias).HasForeignKey(lc => lc.CategoriaId);
    }
    #endregion
}
