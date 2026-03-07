namespace AgremiacionComercio.Application.DTOs.Request;

public class ComercianteFilterRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public string? NombreRazonSocial { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public int? EstadoId { get; set; }
}
