namespace AgremiacionComercio.Domain.Entities;

public abstract class BaseAuditEntity
{
    public DateTime? FechaActualizacion { get; set; }
    public string? UsuarioAuditoria { get; set; }
}
