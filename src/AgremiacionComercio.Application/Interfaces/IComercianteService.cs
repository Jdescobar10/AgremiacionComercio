using AgremiacionComercio.Application.Common;
using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.DTOs.Response;

namespace AgremiacionComercio.Application.Interfaces;

public interface IComercianteService
{
    Task<PagedResponse<ComercianteResponse>> GetPagedAsync(ComercianteFilterRequest filter);
    Task<ComercianteResponse?> GetByIdAsync(int id);
    Task<ComercianteResponse> CreateAsync(CreateComercianteRequest request, string usuarioAuditoria);
    Task<ComercianteResponse?> UpdateAsync(int id, UpdateComercianteRequest request, string usuarioAuditoria);
    Task<bool> DeleteAsync(int id);
    Task<bool> PatchEstadoAsync(int id, PatchEstadoRequest request, string usuarioAuditoria);
}
