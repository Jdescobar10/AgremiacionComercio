using AgremiacionComercio.Application.Interfaces;
using AgremiacionComercio.Domain.Interfaces;
using AgremiacionComercio.Infrastructure.Persistence;
using AgremiacionComercio.Infrastructure.Persistence.Repositories;
using AgremiacionComercio.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AgremiacionComercio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("AgremiacionComercio.Infrastructure")));

        services.AddMemoryCache();

        // Repositorios
        services.AddScoped<IComercianteRepository, ComercianteRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IMunicipioRepository, MunicipioRepository>();

        // Servicios
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IComercianteService, ComercianteService>();
        services.AddScoped<IMunicipioService, MunicipioService>();
        services.AddScoped<IReporteService, ReporteService>();

        return services;
    }
}
