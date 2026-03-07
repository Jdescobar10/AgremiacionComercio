namespace AgremiacionComercio.Application.DTOs.Response;

public class ComercianteResponse
{
    public int ComercianteId { get; set; }
    public string NombreRazonSocial { get; set; } = string.Empty;
    public int MunicipioId { get; set; }
    public string Municipio { get; set; } = string.Empty;
    public int EstadoId { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}
