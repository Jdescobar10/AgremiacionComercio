# AgremiacionComercio API

API REST desarrollada en **.NET 8** con **Clean Architecture** para la gestión de comerciantes y establecimientos de la Agremiación Nacional de Comercio.

## 🏗️ Arquitectura

```
AgremiacionComercio/
├── src/
│   ├── AgremiacionComercio.Domain/          # Entidades, interfaces, reglas de negocio
│   ├── AgremiacionComercio.Application/     # DTOs, servicios, validadores, casos de uso
│   ├── AgremiacionComercio.Infrastructure/  # EF Core, repositorios, servicios externos
│   └── AgremiacionComercio.API/             # Controllers, middleware, configuración
└── tests/
    └── AgremiacionComercio.Tests/           # Unit tests con xUnit + Moq
```

## 🚀 Endpoints

| Método | Ruta | Auth | Rol |
|--------|------|------|-----|
| POST | `/api/auth/login` | ❌ Público | - |
| GET | `/api/municipios` | ✅ JWT | Todos |
| GET | `/api/comerciantes` | ✅ JWT | Todos |
| GET | `/api/comerciantes/{id}` | ✅ JWT | Todos |
| POST | `/api/comerciantes` | ✅ JWT | Todos |
| PUT | `/api/comerciantes/{id}` | ✅ JWT | Todos |
| DELETE | `/api/comerciantes/{id}` | ✅ JWT | Administrador |
| PATCH | `/api/comerciantes/{id}/estado` | ✅ JWT | Todos |
| GET | `/api/reporte/comerciantes` | ✅ JWT | Administrador |
| GET | `/api/reporte/comerciantes/csv` | ✅ JWT | Administrador |

## 🐳 Ejecutar con Docker

```bash
docker-compose up --build
```

La API estará disponible en: `http://localhost:8080`  
Swagger UI: `http://localhost:8080/index.html`

## 💻 Ejecutar en local

### Prerequisitos
- .NET 8 SDK
- SQL Server (local o Docker)

```bash
# Restaurar paquetes
dotnet restore

# Aplicar migraciones (si usas EF Migrations)
cd src/AgremiacionComercio.API
dotnet ef database update --project ../AgremiacionComercio.Infrastructure

# Ejecutar
dotnet run --project src/AgremiacionComercio.API
```

> **Nota:** La base de datos `AgremiacionComercio` debe crearse ejecutando los scripts SQL del repositorio en orden (01 → 02 → 03 → 04) antes de arrancar la API.

## 🧪 Ejecutar Tests

```bash
dotnet test tests/AgremiacionComercio.Tests
```

## 🔐 Usuarios por defecto (datos semilla)

| Correo | Contraseña | Rol |
|--------|-----------|-----|
| admin@agremiacion.com | Admin123! | Administrador |
| auxiliar@agremiacion.com | Auxiliar123! | Auxiliar de Registro |

## 📦 Tecnologías

- .NET 8 / ASP.NET Core
- Entity Framework Core 8 (SQL Server)
- JWT Bearer Authentication
- FluentValidation
- BCrypt.Net
- Swagger / Swashbuckle
- xUnit + Moq + FluentAssertions
- Docker / Docker Compose
