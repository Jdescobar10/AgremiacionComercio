using AgremiacionComercio.Application.DTOs.Response;
using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AgremiacionComercio.Infrastructure.Services;

public class MunicipioService : IMunicipioService
{
    private readonly IMunicipioRepository _repo;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "municipios_all";

    public MunicipioService(IMunicipioRepository repo, IMemoryCache cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<IEnumerable<MunicipioResponse>> GetAllAsync()
    {
        if (_cache.TryGetValue(CacheKey, out IEnumerable<MunicipioResponse>? cached))
            return cached!;

        var municipios = await _repo.GetAllAsync();
        var response = municipios.Select(m => new MunicipioResponse
        {
            MunicipioId = m.MunicipioId,
            Nombre = m.Nombre
        });

        _cache.Set(CacheKey, response, TimeSpan.FromHours(24));
        return response;
    }
}
