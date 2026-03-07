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
    └── AgremiacionComercio.Tests/           # 15 pruebas unitarias xUnit + Moq
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

## 🐳 Ejecutar con Docker (Recomendado)

### Prerequisitos
- Docker Desktop instalado y corriendo

### Pasos

```bash
# 1. Levantar los contenedores (SQL Server 2022 + API)
docker-compose up --build
```

La API estará disponible en: `http://localhost:8080`  
Swagger UI: `http://localhost:8080/index.html`

### 2. Ejecutar los scripts SQL (solo la primera vez)

Una vez levantados los contenedores, copiar y ejecutar los scripts:

```bash
# Copiar scripts al contenedor
docker cp sql/01_Crear_DatabaseTablesIndex.sql agremiacion_sqlserver:/tmp/01.sql
docker cp sql/02_Triggers_Auditoria.sql        agremiacion_sqlserver:/tmp/02.sql
docker cp sql/03_Datos_Semilla.sql             agremiacion_sqlserver:/tmp/03.sql
docker cp sql/04_SP_ReporteComerciantes.sql    agremiacion_sqlserver:/tmp/04.sql

# Entrar al contenedor
docker exec -it agremiacion_sqlserver /bin/bash

# Ejecutar scripts en orden (dentro del contenedor)
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P 'Admin@12345!' -C -i /tmp/01.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P 'Admin@12345!' -C -i /tmp/02.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P 'Admin@12345!' -C -i /tmp/03.sql
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P 'Admin@12345!' -C -i /tmp/04.sql
```

> ✅ Los scripts solo necesitan ejecutarse **una vez** — los datos persisten en el volumen `sqlserver_data`.

## 💻 Ejecutar en local (Visual Studio)

### Prerequisitos
- .NET 8 SDK
- SQL Server local
- Visual Studio 2022 o VS Code

### Pasos

1. Ejecutar los scripts SQL en orden sobre SQL Server local:
   - `sql/01_Crear_DatabaseTablesIndex.sql`
   - `sql/02_Triggers_Auditoria.sql`
   - `sql/03_Datos_Semilla.sql`
   - `sql/04_SP_ReporteComerciantes.sql`

2. Configurar el connection string en `src/AgremiacionComercio.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=AgremiacionComercio;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Restaurar paquetes y ejecutar:

```bash
dotnet restore
dotnet run --project src/AgremiacionComercio.API
```

4. Abrir Swagger UI en: `http://localhost:{puerto}/index.html`

> ⚠️ No se usan migraciones de EF Core — la BD debe crearse con los scripts SQL.

## 🧪 Ejecutar Tests (15/15 ✅)

```bash
dotnet test tests/AgremiacionComercio.Tests --verbosity normal
```

Cobertura:
- `AuthServiceTests` — login BCrypt, migración automática de contraseñas, correo inexistente
- `ComercianteServiceTests` — CRUD completo, PATCH estado, casos de error
- `ValidatorTests` — validaciones FluentValidation (nombre, correo, fecha, municipio)

## 🔐 Usuarios por defecto (datos semilla)

| Correo | Contraseña | Rol |
|--------|-----------|-----|
| admin@agremiacion.com | Admin$2026! | Administrador |
| auxiliar@agremiacion.com | Aux1liar#2026 | Auxiliar de Registro |

> Las contraseñas se migran automáticamente a hash BCrypt en el primer login.

## 📦 Tecnologías

- .NET 8 / ASP.NET Core
- Entity Framework Core 8 (SQL Server) — sin migraciones
- JWT Bearer Authentication — expiración 1 hora
- FluentValidation 11
- BCrypt.Net-Next — hash de contraseñas
- Swagger / Swashbuckle — disponible en todos los ambientes
- IMemoryCache — cache de municipios (24h TTL)
- xUnit + Moq + FluentAssertions — 15 pruebas unitarias
- Docker / Docker Compose — SQL Server 2022 + API

## 🔧 Notas técnicas

- **Triggers SQL:** Las tablas `Comerciante` y `Establecimiento` tienen triggers de auditoría. Se configuró `UseSqlOutputClause(false)` en EF Core para compatibilidad.
- **Swagger en Docker:** Habilitado en todos los ambientes (Development y Production) para facilitar las pruebas.
- **Healthcheck Docker:** Compatibilidad con `mssql-tools18` (SQL Server 2022) y fallback a `mssql-tools`.
- **Auditoría:** `UsuarioAuditoria` se toma del claim `Email` del JWT en cada operación de escritura.
- **Reporte CSV:** Separador pipe `|`, BOM UTF-8 para compatibilidad con Excel, consume `sp_ReporteComerciantes`.
- **SP Reporte:** Modificado para retornar aliases de columnas sin espacios, compatible con EF Core `FromSqlRaw`.
