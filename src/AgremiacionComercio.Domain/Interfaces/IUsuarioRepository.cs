using AgremiacionComercio.Domain.Entities;

namespace AgremiacionComercio.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByCorreoAsync(string correo);
}
