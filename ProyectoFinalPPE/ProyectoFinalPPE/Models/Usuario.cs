namespace ProyectoFinalPPE.Models;


//Tabla de usuarios para el registro y el login
//La constraseña guarda hasheada, nunca en texto plano.

public class Usuario
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

}