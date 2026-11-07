using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalPPE.Dtos;

//DTOs de una autenticación: la misma forma de JSON que espera el cliente Vue
// ({"username": "...", "password": "..."]).

public class RegisterDto
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;

}

public class LoginDto
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string PassWord { get; set; } = string.Empty;

}
