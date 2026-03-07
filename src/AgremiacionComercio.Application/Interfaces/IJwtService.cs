using AgremiacionComercio.Domain.Entities;

namespace AgremiacionComercio.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Usuario usuario);
}
