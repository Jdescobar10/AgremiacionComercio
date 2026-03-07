using AgremiacionComercio.Application.Common;
using AgremiacionComercio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgremiacionComercio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class ReporteController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReporteController(IReporteService reporteService) =>
        _reporteService = reporteService;

    /// <summary>Descarga archivo CSV con comerciantes activos - Solo Administrador</summary>
    [HttpGet("comerciantes/csv")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DescargarCsv()
    {
        var csvBytes = await _reporteService.GenerarCsvComerciantes();
        var fileName = $"reporte_comerciantes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        return File(csvBytes, "text/csv", fileName);
    }

    /// <summary>Obtiene el reporte de comerciantes activos en formato JSON</summary>
    [HttpGet("comerciantes")]
    public async Task<IActionResult> GetReporte()
    {
        var result = await _reporteService.GetReporteAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }
}
