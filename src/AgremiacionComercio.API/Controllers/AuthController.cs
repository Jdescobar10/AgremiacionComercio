using AgremiacionComercio.Application.Common;
using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AgremiacionComercio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequest> _validator;

    public AuthController(IAuthService authService, IValidator<LoginRequest> validator)
    {
        _authService = authService;
        _validator = validator;
    }

    /// <summary>Autenticación de usuario - Retorna JWT Token</summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(
                "Datos inválidos", validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _authService.LoginAsync(request);
        if (result is null)
            return Unauthorized(ApiResponse<object>.Fail("Credenciales incorrectas."));

        return Ok(ApiResponse<object>.Ok(result, "Autenticación exitosa."));
    }
}
