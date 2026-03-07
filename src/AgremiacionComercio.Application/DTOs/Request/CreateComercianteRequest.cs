namespace AgremiacionComercio.Application.DTOs.Request;

public class CreateComercianteRequest
{
    public string NombreRazonSocial { get; set; } = string.Empty;
    public int MunicipioId { get; set; }
    public int EstadoId { get; set; }
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public DateTime FechaRegistro { get; set; }
}
