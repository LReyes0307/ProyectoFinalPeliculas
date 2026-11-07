using Microsoft.EntityFrameworkCore;
using ProyectoFinalPPE.Models;


namespace ProyectoFinalPPE.Data;

// El DbContext es el puente entre las clases de C# y la base de datos SQLite
// Cada BdSet<T> corresponde a una tabla

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pelicula> Peliculas => Set<Pelicula>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //el nombre de usuario debe ser único, igual que en nuestro proyecto anterior
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}


