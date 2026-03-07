using AgremiacionComercio.Domain.Entities;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgremiacionComercio.Infrastructure.Persistence.Repositories;

public class ComercianteRepository : BaseRepository<Comerciante>, IComercianteRepository
{
    public ComercianteRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Comerciante> Items, int Total)> GetPagedAsync(
        int page, int pageSize, string? nombre, DateTime? fechaRegistro, int? estadoId)
    {
        var query = _dbSet
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(c => c.NombreRazonSocial.Contains(nombre));

        if (fechaRegistro.HasValue)
            query = query.Where(c => c.FechaRegistro.Date == fechaRegistro.Value.Date);

        if (estadoId.HasValue)
            query = query.Where(c => c.EstadoId == estadoId.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.FechaRegistro)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public new async Task<Comerciante?> GetByIdAsync(int id) =>
        await _dbSet
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .FirstOrDefaultAsync(c => c.ComercianteId == id);
}
