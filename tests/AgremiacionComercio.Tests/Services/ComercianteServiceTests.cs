using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Domain.Entities;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace AgremiacionComercio.Tests.Services;

public class ComercianteServiceTests
{
    private readonly Mock<IComercianteRepository> _repoMock;
    private readonly IComercianteService _service;

    public ComercianteServiceTests()
    {
        _repoMock = new Mock<IComercianteRepository>();
        _service = new ComercianteService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ComercianteExistente_DebeRetornarResponse()
    {
        // Arrange
        var comerciante = new Comerciante
        {
            ComercianteId = 1,
            NombreRazonSocial = "Empresa Test S.A.S",
            MunicipioId = 1,
            EstadoId = 1,
            FechaRegistro = new DateTime(2024, 1, 15),
            Municipio = new Municipio { MunicipioId = 1, Nombre = "Cali" },
            Estado = new Estado { EstadoId = 1, Nombre = "Activo" }
        };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(comerciante);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.ComercianteId.Should().Be(1);
        result.NombreRazonSocial.Should().Be("Empresa Test S.A.S");
        result.Municipio.Should().Be("Cali");
        result.Estado.Should().Be("Activo");
    }

    [Fact]
    public async Task GetByIdAsync_ComercianteNoExistente_DebeRetornarNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Comerciante?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_DatosValidos_DebeCrearYRetornarComerciante()
    {
        // Arrange
        var request = new CreateComercianteRequest
        {
            NombreRazonSocial = "Nueva Empresa S.A.S",
            MunicipioId = 1,
            EstadoId = 1,
            FechaRegistro = DateTime.Today
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Comerciante>()))
            .ReturnsAsync((Comerciante c) =>
            {
                c.ComercianteId = 10;
                return c;
            });

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Comerciante?)null); // simplificado

        // Act
        var result = await _service.CreateAsync(request, "admin@test.com");

        // Assert
        result.Should().NotBeNull();
        result.NombreRazonSocial.Should().Be("Nueva Empresa S.A.S");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Comerciante>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ComercianteExistente_DebeEliminarYRetornarTrue()
    {
        // Arrange
        var comerciante = new Comerciante
        {
            ComercianteId = 5,
            NombreRazonSocial = "Empresa a Eliminar",
            MunicipioId = 1,
            EstadoId = 1,
            FechaRegistro = DateTime.Today,
            Municipio = new Municipio { Nombre = "Bogotá" },
            Estado = new Estado { Nombre = "Activo" }
        };

        _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(comerciante);
        _repoMock.Setup(r => r.DeleteAsync(It.IsAny<Comerciante>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(5);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Comerciante>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ComercianteNoExistente_DebeRetornarFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Comerciante?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task PatchEstadoAsync_ComercianteExistente_DebeActualizarEstado()
    {
        // Arrange
        var comerciante = new Comerciante
        {
            ComercianteId = 1,
            NombreRazonSocial = "Test",
            MunicipioId = 1,
            EstadoId = 1,
            FechaRegistro = DateTime.Today,
            Municipio = new Municipio { Nombre = "Cali" },
            Estado = new Estado { Nombre = "Activo" }
        };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(comerciante);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Comerciante>())).Returns(Task.CompletedTask);

        var request = new PatchEstadoRequest { EstadoId = 2 };

        // Act
        var result = await _service.PatchEstadoAsync(1, request, "admin@test.com");

        // Assert
        result.Should().BeTrue();
        comerciante.EstadoId.Should().Be(2);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Comerciante>()), Times.Once);
    }
}
