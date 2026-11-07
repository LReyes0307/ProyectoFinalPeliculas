namespace ProyectoFinalPPE.Models;

// El modelo (o entidad) representa la tabla de la base de datos.
// Entity Framework Core crea una tabla "Peliculas" con estas columnas:
public class Pelicula
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Cast { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string AgeRating { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
    public int DurationMinutes { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string OriginalLanguage { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Image { get; set; } = string.Empty;
}

