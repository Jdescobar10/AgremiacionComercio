namespace AgremiacionComercio.Domain.Entities;

public class Municipio
{
    public int MunicipioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Comerciante> Comerciantes { get; set; } = new List<Comerciante>();
}
