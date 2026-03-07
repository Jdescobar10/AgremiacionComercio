using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.Validators;
using FluentAssertions;
using Xunit;

namespace AgremiacionComercio.Tests.Services;

public class ValidatorTests
{
    private readonly CreateComercianteValidator _validator = new();

    [Fact]
    public async Task CreateComerciante_ConDatosValidos_DebeSerValido()
    {
        var request = new CreateComercianteRequest
        {
            NombreRazonSocial = "Empresa Válida S.A.S",
            MunicipioId = 1,
            EstadoId = 1,
            CorreoElectronico = "empresa@test.com",
            FechaRegistro = DateTime.Today
        };

        var result = await _validator.ValidateAsync(request);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task CreateComerciante_SinNombre_DebeSerInvalido()
    {
        var request = new CreateComercianteRequest
        {
            NombreRazonSocial = "",
            MunicipioId = 1,
            EstadoId = 1,
            FechaRegistro = DateTime.Today
        };

        var result = await _validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "NombreRazonSocial");
    }

    [Fact]
    public async Task CreateComerciante_ConCorreoInvalido_DebeSerInvalido()
    {
        var request = new CreateComercianteRequest
        {
            NombreRazonSocial = "Empresa Test",
            MunicipioId = 1,
            EstadoId = 1,
            CorreoElectronico = "no-es-un-correo",
            FechaRegistro = DateTime.Today
        };

        var result = await _validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CorreoElectronico");
    }

    [Fact]
    public async Task CreateComerciante_ConFechaFutura_DebeSerInvalido()
    {
        var request = new CreateComercianteRequest
        {
            NombreRazonSocial = "Empresa Test",
            MunicipioId = 1,
            EstadoId = 1,
            FechaRegistro = DateTime.Today.AddDays(10)
        };

        var result = await _validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FechaRegistro");
    }

    [Fact]
    public async Task CreateComerciante_ConMunicipioInvalido_DebeSerInvalido()
    {
        var request = new CreateComercianteRequest
        {
            NombreRazonSocial = "Empresa Test",
            MunicipioId = 0,
            EstadoId = 1,
            FechaRegistro = DateTime.Today
        };

        var result = await _validator.ValidateAsync(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "MunicipioId");
    }
}
