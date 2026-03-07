using AgremiacionComercio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgremiacionComercio.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario");
        builder.HasKey(x => x.UsuarioId);
        builder.Property(x => x.UsuarioId).UseIdentityColumn();
        builder.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(x => x.CorreoElectronico).HasMaxLength(255).IsRequired();
        builder.HasIndex(x => x.CorreoElectronico).IsUnique();
        builder.Property(x => x.Contrasena).HasMaxLength(255).IsRequired();
        builder.HasOne(x => x.Rol).WithMany(r => r.Usuarios).HasForeignKey(x => x.RolId);
    }
}
