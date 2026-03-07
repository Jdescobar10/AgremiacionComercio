using AgremiacionComercio.Application.Common;
using AgremiacionComercio.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgremiacionComercio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MunicipiosController : ControllerBase
{
    private readonly IMunicipioService _municipioService;

    public MunicipiosController(IMunicipioService municipioService) =>
        _municipioService = municipioService;

    /// <summary>Retorna lista de municipios disponibles (con cache en memoria)</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _municipioService.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(result));
    }
}
