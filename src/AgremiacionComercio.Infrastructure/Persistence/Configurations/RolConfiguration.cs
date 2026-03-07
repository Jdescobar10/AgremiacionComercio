using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgremiacionComercio.Infrastructure.Persistence.Configurations;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Rol");
        builder.HasKey(x => x.RolId);
        builder.Property(x => x.RolId).UseIdentityColumn();
        builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
    }
}
