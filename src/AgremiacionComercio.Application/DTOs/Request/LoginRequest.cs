namespace AgremiacionComercio.Application.DTOs.Request;

public class LoginRequest
{
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}
