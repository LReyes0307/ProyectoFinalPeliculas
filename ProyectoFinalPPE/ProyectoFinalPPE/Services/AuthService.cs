using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalPPE.Data;
using ProyectoFinalPPE.Models;

namespace ProyectoFinalPPE.Services;
// Servicio de autenticación: registro con contraseña haseada
// y el login que emite un JWT

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<Usuario> Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("El usuario y la contraseña son obligatorios");
        }

        var trimmed = username.Trim();
        var exists = await _context.Usuarios.AnyAsync(u => u.Username == trimmed);
        if (exists)
        {
            throw new InvalidOperationException("El nombre de usuario ya está registrado");
        }

        // Nunca se guarda la contraseña en texto plano: se guarda su hash.
        var usuario = new Usuario
        {
            Username = trimmed,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<string?> Login(string username, string password)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

        // Se compara la contraseña contra el hash guardado.
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
        {
            return null;
        }

        return GenerateToken(usuario);
    }

    // El JWT lleva el id (sub) y el username, firmado con la clave secreta.
    private string GenerateToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("sub", usuario.Id.ToString()),
            new Claim("username", usuario.Username),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
