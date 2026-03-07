using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgremiacionComercio.Infrastructure.Persistence.Configurations;

public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.ToTable("Municipio");
        builder.HasKey(x => x.MunicipioId);
        builder.Property(x => x.MunicipioId).UseIdentityColumn();
        builder.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => x.Nombre);
    }
}
