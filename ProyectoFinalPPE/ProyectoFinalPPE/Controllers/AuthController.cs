using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalPPE.Dtos;
using ProyectoFinalPPE.Services;

namespace ProyectoFinalPPE.Controllers;

// Mismos endpoints que la API de NestJS:
//   POST /auth/register  -> crea el usuario con la contraseña hasheada
//   POST /auth/login     -> devuelve { "access_token": "..." }
//   GET  /auth/profile   -> devuelve los datos del token (protegido)
[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _service;

    public AuthController(AuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var usuario = await _service.Register(dto.UserName, dto.Password);
            // Nunca se devuelve la contraseña, ni siquiera el hash.
            return StatusCode(201, new { usuario.Id, usuario.Username });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _service.Login(dto.UserName, dto.PassWord);
        if (token is null)
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        // El cliente Vue lee la propiedad "access_token", por eso se
        // respeta ese nombre exacto en lugar del camelCase habitual.
        return Ok(new LoginResponse { AccessToken = token });
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult Profile()
    {
        return Ok(new
        {
            sub = User.FindFirst("sub")?.Value,
            username = User.FindFirst("username")?.Value,
        });
    }

    private class LoginResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
    }
}
