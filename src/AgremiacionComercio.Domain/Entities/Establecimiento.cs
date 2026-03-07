namespace AgremiacionComercio.Domain.Entities;

public class Establecimiento : BaseAuditEntity
{
    public int EstablecimientoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Ingresos { get; set; }
    public int NumeroEmpleados { get; set; }
    public int ComercianteId { get; set; }
    public Comerciante Comerciante { get; set; } = null!;
}
