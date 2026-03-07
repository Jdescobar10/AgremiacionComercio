using AgremiacionComercio.Application.DTOs.Response;

namespace AgremiacionComercio.Application.Interfaces;

public interface IMunicipioService
{
    Task<IEnumerable<MunicipioResponse>> GetAllAsync();
}
