using AgremiacionComercio.Domain.Entities;

namespace AgremiacionComercio.Domain.Interfaces;

public interface IMunicipioRepository
{
    Task<IEnumerable<Municipio>> GetAllAsync();
}
