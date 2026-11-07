namespace ProyectoFinalPPE.Dtos;

// Dto de creacion: define la forma del JSON que el cliente envía al crear
// El id no va porque lo genera la base de datos.
// Los nombres se serializan en camelCase automáticamente (title, director, ...)

public class CreatePeliculaDto
{
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Cast { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string AgeRating { get; set; } = string.Empty;
    public string? ReleaseDate { get; set; }
    public int DurationMinutes { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string OriginalLanguage { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Image { get; set; } = string.Empty;
}
