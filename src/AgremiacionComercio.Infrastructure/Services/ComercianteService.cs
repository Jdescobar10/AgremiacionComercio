using AgremiacionComercio.Application.Common;
using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.DTOs.Response;
using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Domain.Entities;
using AgremiacionComercio.Domain.Interfaces;

namespace AgremiacionComercio.Infrastructure.Services;

public class ComercianteService : IComercianteService
{
    private readonly IComercianteRepository _repo;

    public ComercianteService(IComercianteRepository repo) => _repo = repo;

    public async Task<PagedResponse<ComercianteResponse>> GetPagedAsync(ComercianteFilterRequest filter)
    {
        var (items, total) = await _repo.GetPagedAsync(
            filter.Page, filter.PageSize,
            filter.NombreRazonSocial, filter.FechaRegistro, filter.EstadoId);

        return new PagedResponse<ComercianteResponse>
        {
            Items = items.Select(MapToResponse),
            Page = filter.Page,
            PageSize = filter.PageSize,
            Total = total
        };
    }

    public async Task<ComercianteResponse?> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        return c is null ? null : MapToResponse(c);
    }

    public async Task<ComercianteResponse> CreateAsync(CreateComercianteRequest request, string usuarioAuditoria)
    {
        var entity = new Comerciante
        {
            NombreRazonSocial = request.NombreRazonSocial,
            MunicipioId = request.MunicipioId,
            EstadoId = request.EstadoId,
            Telefono = request.Telefono,
            CorreoElectronico = request.CorreoElectronico,
            FechaRegistro = request.FechaRegistro,
            UsuarioAuditoria = usuarioAuditoria,
            FechaActualizacion = DateTime.Now
        };

        await _repo.AddAsync(entity);

        // Recargar desde BD con las relaciones incluidas (Municipio, Estado)
        var creado = await _repo.GetByIdAsync(entity.ComercianteId);
        return creado is not null ? MapToResponse(creado) : MapToResponse(entity);
    }

    public async Task<ComercianteResponse?> UpdateAsync(int id, UpdateComercianteRequest request, string usuarioAuditoria)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return null;

        entity.NombreRazonSocial = request.NombreRazonSocial;
        entity.MunicipioId = request.MunicipioId;
        entity.EstadoId = request.EstadoId;
        entity.Telefono = request.Telefono;
        entity.CorreoElectronico = request.CorreoElectronico;
        entity.FechaRegistro = request.FechaRegistro;
        entity.UsuarioAuditoria = usuarioAuditoria;
        entity.FechaActualizacion = DateTime.Now;

        await _repo.UpdateAsync(entity);
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;
        await _repo.DeleteAsync(entity);
        return true;
    }

    public async Task<bool> PatchEstadoAsync(int id, PatchEstadoRequest request, string usuarioAuditoria)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) return false;

        entity.EstadoId = request.EstadoId;
        entity.UsuarioAuditoria = usuarioAuditoria;
        entity.FechaActualizacion = DateTime.Now;

        await _repo.UpdateAsync(entity);
        return true;
    }

    private static ComercianteResponse MapToResponse(Comerciante c) => new()
    {
        ComercianteId = c.ComercianteId,
        NombreRazonSocial = c.NombreRazonSocial,
        MunicipioId = c.MunicipioId,
        Municipio = c.Municipio?.Nombre ?? string.Empty,
        EstadoId = c.EstadoId,
        Estado = c.Estado?.Nombre ?? string.Empty,
        Telefono = c.Telefono,
        CorreoElectronico = c.CorreoElectronico,
        FechaRegistro = c.FechaRegistro,
        FechaActualizacion = c.FechaActualizacion
    };
}
