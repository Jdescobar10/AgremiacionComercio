using System.Security.Claims;
using AgremiacionComercio.Application.Common;
using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgremiacionComercio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComerciantesController : ControllerBase
{
    private readonly IComercianteService _service;
    private readonly IValidator<CreateComercianteRequest> _createValidator;
    private readonly IValidator<UpdateComercianteRequest> _updateValidator;

    public ComerciantesController(
        IComercianteService service,
        IValidator<CreateComercianteRequest> createValidator,
        IValidator<UpdateComercianteRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>Consulta paginada de comerciantes con filtros opcionales</summary>
    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] ComercianteFilterRequest filter)
    {
        var result = await _service.GetPagedAsync(filter);
        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>Obtiene un comerciante por su Id</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Comerciante con Id {id} no encontrado."));
        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>Crea un nuevo comerciante</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateComercianteRequest request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(
                "Datos inválidos", validation.Errors.Select(e => e.ErrorMessage)));

        var usuario = GetUsuarioAuditoria();
        var result = await _service.CreateAsync(request, usuario);
        return CreatedAtAction(nameof(GetById), new { id = result.ComercianteId },
            ApiResponse<object>.Ok(result, "Comerciante creado exitosamente."));
    }

    /// <summary>Actualiza un comerciante existente</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateComercianteRequest request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(
                "Datos inválidos", validation.Errors.Select(e => e.ErrorMessage)));

        var usuario = GetUsuarioAuditoria();
        var result = await _service.UpdateAsync(id, request, usuario);
        if (result is null)
            return NotFound(ApiResponse<object>.Fail($"Comerciante con Id {id} no encontrado."));

        return Ok(ApiResponse<object>.Ok(result, "Comerciante actualizado exitosamente."));
    }

    /// <summary>Elimina un comerciante - Solo Administrador</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<object>.Fail($"Comerciante con Id {id} no encontrado."));

        return Ok(ApiResponse<object>.OkMessage("Comerciante eliminado exitosamente."));
    }

    /// <summary>Modifica el estado de un comerciante</summary>
    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> PatchEstado(int id, [FromBody] PatchEstadoRequest request)
    {
        var usuario = GetUsuarioAuditoria();
        var updated = await _service.PatchEstadoAsync(id, request, usuario);
        if (!updated)
            return NotFound(ApiResponse<object>.Fail($"Comerciante con Id {id} no encontrado."));

        return Ok(ApiResponse<object>.OkMessage("Estado actualizado exitosamente."));
    }

    private string GetUsuarioAuditoria() =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? "sistema";
}
