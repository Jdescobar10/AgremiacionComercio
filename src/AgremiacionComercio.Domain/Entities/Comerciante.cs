namespace AgremiacionComercio.Domain.Entities;

public class Comerciante : BaseAuditEntity
{
    public int ComercianteId { get; set; }
    public string NombreRazonSocial { get; set; } = string.Empty;
    public int MunicipioId { get; set; }
    public int EstadoId { get; set; }
    public string? Telefono { get; set; }
    public string? CorreoElectronico { get; set; }
    public DateTime FechaRegistro { get; set; }
    public Municipio Municipio { get; set; } = null!;
    public Estado Estado { get; set; } = null!;
    public ICollection<Establecimiento> Establecimientos { get; set; } = new List<Establecimiento>();
}
