using System.Text;
using AgremiacionComercio.Application.DTOs.Response;
using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgremiacionComercio.Infrastructure.Services;

public class ReporteService : IReporteService
{
    private readonly AppDbContext _context;

    public ReporteService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<ReporteComercianteResponse>> GetReporteAsync()
    {
        // Consume el SP sp_ReporteComerciantes del Reto 4 SQL
        var result = await _context.Database
            .SqlQueryRaw<ReporteComercianteRaw>(
                "EXEC dbo.sp_ReporteComerciantes")
            .ToListAsync();

        return result.Select(r => new ReporteComercianteResponse
        {
            NombreRazonSocial = r.NombreRazonSocial,
            Municipio = r.Municipio,
            Telefono = r.Telefono,
            CorreoElectronico = r.CorreoElectronico,
            FechaRegistro = r.FechaRegistro,
            Estado = r.Estado,
            CantidadEstablecimientos = r.CantidadEstablecimientos,
            TotalIngresos = r.TotalIngresos,
            CantidadEmpleados = r.CantidadEmpleados
        });
    }

    public async Task<byte[]> GenerarCsvComerciantes()
    {
        var data = await GetReporteAsync();

        var sb = new StringBuilder();
        // Header
        sb.AppendLine("NombreRazonSocial|Municipio|Telefono|CorreoElectronico|FechaRegistro|Estado|CantidadEstablecimientos|TotalIngresos|CantidadEmpleados");

        // Rows
        foreach (var r in data)
        {
            sb.AppendLine(
                $"{r.NombreRazonSocial}|" +
                $"{r.Municipio}|" +
                $"{r.Telefono ?? "N/A"}|" +
                $"{r.CorreoElectronico ?? "N/A"}|" +
                $"{r.FechaRegistro:yyyy-MM-dd}|" +
                $"{r.Estado}|" +
                $"{r.CantidadEstablecimientos}|" +
                $"{r.TotalIngresos:F2}|" +
                $"{r.CantidadEmpleados}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    // Clase interna para mapear el resultado del SP
    private class ReporteComercianteRaw
    {
        public string NombreRazonSocial { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int CantidadEstablecimientos { get; set; }
        public decimal TotalIngresos { get; set; }
        public int CantidadEmpleados { get; set; }
    }
}
