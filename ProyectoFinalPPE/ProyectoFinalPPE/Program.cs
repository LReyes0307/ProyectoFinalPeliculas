using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalPPE.Data;
using ProyectoFinalPPE.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios de la aplicación registrados en el contenedor de inyección
// de dependencias. Todo lo que se registra aquí se puede "inyectar"
// en los constructores de controladores y servicios.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework Core con SQLite: la cadena de conexión indica el
// archivo de base de datos (peliculas.db). SQLite es portable, no hace
// falta instalar ningún servidor, así corre en cualquier computador.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Los servicios con la lógica de negocio, registrados por inyección
// de dependencias: ASP.NET Core crea una instancia por petición y la
// entrega a los controladores que la piden en su constructor.
builder.Services.AddScoped<PeliculasService>();
builder.Services.AddScoped<AuthService>();

// Autenticación con JWT Bearer: así el servidor valida el token que el
// cliente envía en el encabezado "Authorization: Bearer <token>".
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Conserva los nombres originales de los claims del token (sub, username).
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        };
    });

builder.Services.AddAuthorization();

// CORS: la API solo acepta peticiones del origen del cliente Vue
// (http://localhost:5173), igual que hacía el backend de NestJS.
builder.Services.AddCors(options =>
{
    options.AddPolicy("vue", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS va antes de la autenticación: primero se acepta el origen,
// luego se valida el token.
app.UseCors("vue");

// El orden importa: primero UseAuthentication (lee y valida el token)
// y después UseAuthorization (decide si puede pasar).
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
