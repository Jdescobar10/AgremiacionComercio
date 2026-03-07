# ── Stage 1: Build ──
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar .csproj y restaurar dependencias (cache layer)
COPY ["src/AgremiacionComercio.API/AgremiacionComercio.API.csproj", "src/AgremiacionComercio.API/"]
COPY ["src/AgremiacionComercio.Application/AgremiacionComercio.Application.csproj", "src/AgremiacionComercio.Application/"]
COPY ["src/AgremiacionComercio.Infrastructure/AgremiacionComercio.Infrastructure.csproj", "src/AgremiacionComercio.Infrastructure/"]
COPY ["src/AgremiacionComercio.Domain/AgremiacionComercio.Domain.csproj", "src/AgremiacionComercio.Domain/"]

RUN dotnet restore "src/AgremiacionComercio.API/AgremiacionComercio.API.csproj"

# Copiar todo el código y publicar
COPY . .
WORKDIR "/src/src/AgremiacionComercio.API"
RUN dotnet publish "AgremiacionComercio.API.csproj" -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime ──
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Usuario no-root por seguridad
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "AgremiacionComercio.API.dll"]
