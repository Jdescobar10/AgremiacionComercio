namespace AgremiacionComercio.Domain.Entities;

public class Estado
{
    public int EstadoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Comerciante> Comerciantes { get; set; } = new List<Comerciante>();
}
