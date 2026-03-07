using AgremiacionComercio.Domain.Entities;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgremiacionComercio.Infrastructure.Persistence.Repositories;

public class MunicipioRepository : IMunicipioRepository
{
    private readonly AppDbContext _context;

    public MunicipioRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Municipio>> GetAllAsync() =>
        await _context.Municipios.OrderBy(m => m.Nombre).ToListAsync();
}
