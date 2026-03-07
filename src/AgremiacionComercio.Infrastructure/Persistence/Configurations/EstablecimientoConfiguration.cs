using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgremiacionComercio.Infrastructure.Persistence.Configurations;

public class EstablecimientoConfiguration : IEntityTypeConfiguration<Establecimiento>
{
    public void Configure(EntityTypeBuilder<Establecimiento> builder)
    {
        builder.ToTable("Establecimiento", tb => tb.UseSqlOutputClause(false));
        builder.HasKey(x => x.EstablecimientoId);
        builder.Property(x => x.EstablecimientoId).UseIdentityColumn();
        builder.Property(x => x.Nombre).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Ingresos).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.NumeroEmpleados).IsRequired();
        builder.Property(x => x.FechaActualizacion).HasColumnType("datetime2(0)");
        builder.Property(x => x.UsuarioAuditoria).HasMaxLength(255);

        builder.HasOne(x => x.Comerciante).WithMany(c => c.Establecimientos).HasForeignKey(x => x.ComercianteId);
    }
}
