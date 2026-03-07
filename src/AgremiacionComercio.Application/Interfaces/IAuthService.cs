using AgremiacionComercio.Application.DTOs.Request;
using AgremiacionComercio.Application.DTOs.Response;

namespace AgremiacionComercio.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
