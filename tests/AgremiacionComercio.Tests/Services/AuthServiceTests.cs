using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Domain.Entities;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;
using AgremiacionComercio.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AgremiacionComercio.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AppDbContext _dbContext;
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        _usuarioRepoMock = new Mock<IUsuarioRepository>();
        _jwtServiceMock  = new Mock<IJwtService>();

        // DbContext en memoria para pruebas unitarias
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new AppDbContext(options);

        _authService = new AuthService(
            _usuarioRepoMock.Object,
            _jwtServiceMock.Object,
            _dbContext);
    }

    [Fact]
    public async Task Login_ConCredencialesValidas_BCrypt_DebeRetornarToken()
    {
        // Arrange — contraseña ya hasheada con BCrypt (flujo normal post-migración)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin$2026!");
        var usuario = new Usuario
        {
            UsuarioId = 1,
            Nombre    = "Admin Test",
            CorreoElectronico = "admin@test.com",
            Contrasena = passwordHash,
            RolId = 1,
            Rol = new Rol { RolId = 1, Nombre = "Administrador" }
        };

        _usuarioRepoMock
            .Setup(r => r.GetByCorreoAsync("admin@test.com"))
            .ReturnsAsync(usuario);

        _jwtServiceMock
            .Setup(j => j.GenerateToken(It.IsAny<Usuario>()))
            .Returns("fake-jwt-token");

        var request = new LoginRequest
        {
            CorreoElectronico = "admin@test.com",
            Contrasena        = "Admin$2026!"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().Be("fake-jwt-token");
        result.NombreUsuario.Should().Be("Admin Test");
        result.Rol.Should().Be("Administrador");
    }

    [Fact]
    public async Task Login_ConContrasenaTextoplano_DebeMigrarYRetornarToken()
    {
        // Arrange — contraseña en texto plano (datos semilla sin migrar aún)
        var rol = new Rol { RolId = 2, Nombre = "Auxiliar de Registro" };
        var usuario = new Usuario
        {
            UsuarioId = 2,
            Nombre    = "Auxiliar Test",
            CorreoElectronico = "auxiliar@test.com",
            Contrasena = "Aux1liar#2026",   // texto plano
            RolId = 2,
            Rol = rol
        };

        // Registrar en el DbContext InMemory para que SaveChangesAsync funcione
        _dbContext.Set<Rol>().Add(rol);
        _dbContext.Set<Usuario>().Add(usuario);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();

        _usuarioRepoMock
            .Setup(r => r.GetByCorreoAsync("auxiliar@test.com"))
            .ReturnsAsync(usuario);

        _jwtServiceMock
            .Setup(j => j.GenerateToken(It.IsAny<Usuario>()))
            .Returns("fake-jwt-token-aux");

        var request = new LoginRequest
        {
            CorreoElectronico = "auxiliar@test.com",
            Contrasena        = "Aux1liar#2026"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert — login exitoso
        result.Should().NotBeNull();
        result!.Token.Should().Be("fake-jwt-token-aux");

        // Verificar que la contraseña fue migrada a BCrypt
        usuario.Contrasena.Should().StartWith("$2");
        BCrypt.Net.BCrypt.Verify("Aux1liar#2026", usuario.Contrasena).Should().BeTrue();
    }

    [Fact]
    public async Task Login_ConContrasenaIncorrecta_DebeRetornarNull()
    {
        // Arrange
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("ContraseñaCorrecta");
        var usuario = new Usuario
        {
            UsuarioId = 1,
            Nombre    = "Admin Test",
            CorreoElectronico = "admin@test.com",
            Contrasena = passwordHash,
            RolId = 1,
            Rol = new Rol { RolId = 1, Nombre = "Administrador" }
        };

        _usuarioRepoMock
            .Setup(r => r.GetByCorreoAsync("admin@test.com"))
            .ReturnsAsync(usuario);

        var request = new LoginRequest
        {
            CorreoElectronico = "admin@test.com",
            Contrasena        = "ContraseñaIncorrecta"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Login_ConCorreoInexistente_DebeRetornarNull()
    {
        // Arrange
        _usuarioRepoMock
            .Setup(r => r.GetByCorreoAsync(It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        var request = new LoginRequest
        {
            CorreoElectronico = "noexiste@test.com",
            Contrasena        = "cualquierpassword"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        result.Should().BeNull();
    }
}
