using AgremiacionComercio.Domain.Entities;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgremiacionComercio.Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) => _context = context;

    public async Task<Usuario?> GetByCorreoAsync(string correo) =>
        await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.CorreoElectronico == correo);
}
