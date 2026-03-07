using AgremiacionComercio.Domain.Entities;

namespace AgremiacionComercio.Domain.Interfaces;

public interface IComercianteRepository : IRepository<Comerciante>
{
    Task<(IEnumerable<Comerciante> Items, int Total)> GetPagedAsync(
        int page, int pageSize, string? nombre, DateTime? fechaRegistro, int? estadoId);
}
