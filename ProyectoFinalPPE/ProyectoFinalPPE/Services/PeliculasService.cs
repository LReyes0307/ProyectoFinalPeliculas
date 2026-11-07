using Microsoft.EntityFrameworkCore;
using ProyectoFinalPPE.Data;
using ProyectoFinalPPE.Dtos;
using ProyectoFinalPPE.Models;

namespace ProyectoFinalPPE.Services;

// El servicio concentra la lógica de negocio: es la única clase que
// manipula el modelo. El controlador no habla directo con la base de datos.
// Se registra por inyección de dependencias en Program.cs.
public class PeliculasService
{
    private readonly AppDbContext _context;

    public PeliculasService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pelicula>> GetAll(string? search = null, int? page = null, int? pageSize = null)
    {
        // Sin parámetros devuelve la lista completa ordenada por id, exactamente
        // como la API de NestJS. Opcionalmente acepta búsqueda por nombre
        // (?search=) y paginación (?page=&pageSize=) sin cambiar la forma
        // de la respuesta: siempre es un arreglo de películas.
        var query = _context.Peliculas.OrderBy(p => p.Id).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Title.ToLower().Contains(search.Trim().ToLower()));
        }

        if (page is not null && pageSize is not null && page > 0 && pageSize > 0)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<Pelicula?> GetById(int id)
    {
        return await _context.Peliculas.FindAsync(id);
    }

    public async Task<Pelicula> Create(CreatePeliculaDto dto)
    {
        var pelicula = new Pelicula
        {
            Title = dto.Title,
            Director = dto.Director,
            Cast = dto.Cast,
            Genre = dto.Genre,
            AgeRating = dto.AgeRating,
            ReleaseDate = ParseReleaseDate(dto.ReleaseDate),
            DurationMinutes = dto.DurationMinutes,
            Synopsis = dto.Synopsis,
            Country = dto.Country,
            OriginalLanguage = dto.OriginalLanguage,
            Rating = dto.Rating,
            Image = dto.Image,
        };

        _context.Peliculas.Add(pelicula);
        await _context.SaveChangesAsync();
        return pelicula;
    }

    public async Task<Pelicula?> Update(int id, UpdatePeliculaDto dto)
    {
        var pelicula = await _context.Peliculas.FindAsync(id);
        if (pelicula is null) return null;

        // Solo se aplican los campos que vienen en el JSON (PATCH parcial).
        if (dto.Title is not null) pelicula.Title = dto.Title;
        if (dto.Director is not null) pelicula.Director = dto.Director;
        if (dto.Cast is not null) pelicula.Cast = dto.Cast;
        if (dto.Genre is not null) pelicula.Genre = dto.Genre;
        if (dto.AgeRating is not null) pelicula.AgeRating = dto.AgeRating;
        if (dto.ReleaseDate is not null) pelicula.ReleaseDate = ParseReleaseDate(dto.ReleaseDate);
        if (dto.DurationMinutes is not null) pelicula.DurationMinutes = dto.DurationMinutes.Value;
        if (dto.Synopsis is not null) pelicula.Synopsis = dto.Synopsis;
        if (dto.Country is not null) pelicula.Country = dto.Country;
        if (dto.OriginalLanguage is not null) pelicula.OriginalLanguage = dto.OriginalLanguage;
        if (dto.Rating is not null) pelicula.Rating = dto.Rating.Value;
        if (dto.Image is not null) pelicula.Image = dto.Image;

        await _context.SaveChangesAsync();
        return pelicula;
    }

    public async Task<Pelicula?> Delete(int id)
    {
        var pelicula = await _context.Peliculas.FindAsync(id);
        if (pelicula is null) return null;

        _context.Peliculas.Remove(pelicula);
        await _context.SaveChangesAsync();
        return pelicula;
    }

    // Misma regla que en NestJS: fecha vacía o ausente -> null;
    // fecha inválida -> error 400 con el mismo mensaje.
    private static DateTime? ParseReleaseDate(string? releaseDate)
    {
        if (string.IsNullOrEmpty(releaseDate)) return null;

        if (!DateTime.TryParse(releaseDate, out var parsed))
        {
            throw new ArgumentException("releaseDate debe ser una fecha válida");
        }
        return parsed;
    }
}
