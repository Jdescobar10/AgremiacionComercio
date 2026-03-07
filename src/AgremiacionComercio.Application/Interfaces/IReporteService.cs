using AgremiacionComercio.Application.DTOs.Response;

namespace AgremiacionComercio.Application.Interfaces;

public interface IReporteService
{
    Task<byte[]> GenerarCsvComerciantes();
    Task<IEnumerable<ReporteComercianteResponse>> GetReporteAsync();
}
