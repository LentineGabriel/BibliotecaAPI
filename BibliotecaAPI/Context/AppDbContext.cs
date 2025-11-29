#region USINGS
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BibliotecaAPI.Context;
public class AppDbContext : DbContext
{
    #region PROPS/CTOR
    public DbSet<Livros> Livros { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}
