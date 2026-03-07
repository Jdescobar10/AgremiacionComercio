using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgremiacionComercio.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Municipio> Municipios => Set<Municipio>();
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Comerciante> Comerciantes => Set<Comerciante>();
    public DbSet<Establecimiento> Establecimientos => Set<Establecimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
