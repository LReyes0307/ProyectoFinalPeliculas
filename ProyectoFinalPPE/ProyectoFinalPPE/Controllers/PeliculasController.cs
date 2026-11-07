using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalPPE.Dtos;
using ProyectoFinalPPE.Services;

namespace ProyectoFinalPPE.Controllers;

// Mismas rutas, verbos y códigos de estado que la API de NestJS:
// GET  /peliculas
// GET  /peliculas/{id}
// POST /peliculas      (protegido con JWT)
// PATCH    /peliculas/{id}      (protegido con JWT)
// DELETE   /peliculas      (protegido con JWT)

[ApiController]
[Route("peliculas")]
public class PeliculasController : ControllerBase
{
    private readonly PeliculasService _service;

    public PeliculasController(PeliculasService service)
    {
        _service = service;
    }

    // Parámetros opcionales: ?search= (buscar por nombre) y ?page=&pageSize=
    // (paginación). Sin parámetros devuelve la lista completa, igual que Nest.
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        return Ok(await _service.GetAll(search, page, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pelicula = await _service.GetById(id);
        if (pelicula is null)
        {
            return NotFound(new { message = $"No existe la pelicula con id {id}" });
        }
        return Ok(pelicula);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreatePeliculaDto dto)
    {
        try
        {
            var pelicula = await _service.Create(dto);
            return StatusCode(201, pelicula);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePeliculaDto dto)
    {
        try
        {
            var pelicula = await _service.Update(id, dto);
            if (pelicula is null)
            {
                return NotFound(new { message = $"No existe la pelicula con id {id}" });
            }
            return Ok(pelicula);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pelicula = await _service.Delete(id);
        if (pelicula is null)
        {
            return NotFound(new { message = $"No existe la pelicula con id {id}" });
        }
        return Ok(pelicula);
    }
}

