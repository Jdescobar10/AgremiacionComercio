namespace AgremiacionComercio.Application.DTOs.Response;

public class ReporteComercianteResponse
{
    public string NombreRazonSocial { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int CantidadEstablecimientos { get; set; }
    public decimal TotalIngresos { get; set; }
    public int CantidadEmpleados { get; set; }
}
