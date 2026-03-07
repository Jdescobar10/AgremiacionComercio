using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgremiacionComercio.Infrastructure.Persistence.Configurations;

public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("Estado");
        builder.HasKey(x => x.EstadoId);
        builder.Property(x => x.EstadoId).UseIdentityColumn();
        builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
    }
}
