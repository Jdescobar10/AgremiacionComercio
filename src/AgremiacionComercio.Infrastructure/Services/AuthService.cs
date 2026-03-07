using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.DTOs.Response;
using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;

namespace AgremiacionComercio.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IJwtService _jwtService;
    private readonly AppDbContext _context;

    public AuthService(IUsuarioRepository usuarioRepo, IJwtService jwtService, AppDbContext context)
    {
        _usuarioRepo = usuarioRepo;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepo.GetByCorreoAsync(request.CorreoElectronico);
        if (usuario is null) return null;

        bool passwordValida;

        // Si la contraseña en BD es un hash BCrypt (empieza con $2a$ o $2b$)
        if (usuario.Contrasena.StartsWith("$2a$") || usuario.Contrasena.StartsWith("$2b$"))
        {
            passwordValida = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.Contrasena);
        }
        else
        {
            // Contraseña en texto plano — comparar directamente
            // y si es correcta, migrar automáticamente a hash BCrypt
            passwordValida = usuario.Contrasena == request.Contrasena;

            if (passwordValida)
            {
                // Migrar a hash BCrypt automáticamente
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(request.Contrasena);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
        }

        if (!passwordValida) return null;

        var token = _jwtService.GenerateToken(usuario);

        return new LoginResponse
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(1),
            NombreUsuario = usuario.Nombre,
            Rol = usuario.Rol.Nombre
        };
    }
}

