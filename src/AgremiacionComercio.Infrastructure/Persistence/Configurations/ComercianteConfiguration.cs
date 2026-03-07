using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgremiacionComercio.Infrastructure.Persistence.Configurations;

public class ComercianteConfiguration : IEntityTypeConfiguration<Comerciante>
{
    public void Configure(EntityTypeBuilder<Comerciante> builder)
    {
        builder.ToTable("Comerciante", tb => tb.UseSqlOutputClause(false));
        builder.HasKey(x => x.ComercianteId);
        builder.Property(x => x.ComercianteId).UseIdentityColumn();
        builder.Property(x => x.NombreRazonSocial).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Telefono).HasMaxLength(20);
        builder.Property(x => x.CorreoElectronico).HasMaxLength(255);
        builder.Property(x => x.FechaRegistro).HasColumnType("date").IsRequired();
        builder.Property(x => x.FechaActualizacion).HasColumnType("datetime2(0)");
        builder.Property(x => x.UsuarioAuditoria).HasMaxLength(255);

        builder.HasIndex(x => x.NombreRazonSocial);
        builder.HasIndex(x => x.FechaRegistro);
        builder.HasIndex(x => x.EstadoId);

        builder.HasOne(x => x.Municipio).WithMany(m => m.Comerciantes).HasForeignKey(x => x.MunicipioId);
        builder.HasOne(x => x.Estado).WithMany(e => e.Comerciantes).HasForeignKey(x => x.EstadoId);
    }
}
