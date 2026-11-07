namespace ProyectoFinalPPE.Dtos;

//Dto de actualizacion (PATCH): todos los campos son opcionales
// Solo se actualizan los campos que el  cliente envíe

public class UpdatePeliculaDto
{
    public string? Title { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? Genre { get; set; }
    public string? AgeRating { get; set; }
    public string? ReleaseDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Synopsis { get; set; }
    public string? Country { get; set; }
    public string? OriginalLanguage { get; set; }
    public double? Rating { get; set; }
    public string? Image { get; set; }
}

