# WorkforcePortal — Backend

A .NET (net10.0) backend for the Workforce Management Portal, built with a clean
architecture style split across four projects:

- **WorkforcePortal.Domain** — entities, enums, domain exceptions
- **WorkforcePortal.Application** — DTOs, services, validators, AutoMapper profiles, interfaces
- **WorkforcePortal.Infrastructure** — EF Core `AppDbContext`, repositories, JWT/password services
- **WorkforcePortal.Api** *(your ASP.NET Core entry-point project, hosts `Program.cs`)*

Key tech: EF Core 9 + SQL Server, AutoMapper, FluentValidation, JWT auth (`System.IdentityModel.Tokens.Jwt`), BCrypt.Net for password hashing.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for running SQL Server locally)
- EF Core CLI tools:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## 1. Start SQL Server (Docker)

Run a local SQL Server 2022 container:

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrongPassword123!" \
  -p 1433:1433 \
  --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Verify it's running:

```bash
docker ps
```

To stop/start it later:

```bash
docker stop sqlserver
docker start sqlserver
```

## 2. Configure the connection string & JWT settings

In your API project's `appsettings.Development.json` (create it if it doesn't exist):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=WorkforcePortalDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY",
    "Issuer": "WorkforcePortal",
    "Audience": "WorkforcePortal",
    "ExpiryMinutes": "3000"
  }
}
```

> The JWT secret key should be at least 32 characters for HMAC-SHA256 signing.

## 3. Restore & build

From the solution root:

```bash
dotnet restore
dotnet build
```

## 4. Apply EF Core migrations

If migrations don't exist yet, create the initial one (run from the API project directory, or pass `--project`/`--startup-project`):

```bash
dotnet ef migrations add InitialCreate \
  --project WorkforcePortal.Infrastructure \
  --startup-project WorkforcePortal.Api

dotnet ef database update \
  --project WorkforcePortal.Infrastructure \
  --startup-project WorkforcePortal.Api
```

The app also seeds initial data (departments, an employee, an admin user, a sample task) via `DatabaseSeeder.SeedAsync` — wire this into `Program.cs` on startup if not already:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}
```

Default seeded admin login: **username:** `admin` · **password:** `Admin123!`

## 5. Run the API

```bash
dotnet run --project WorkforcePortal.Api
```

By default the API will be available at `https://localhost:5012` (or whatever port is configured) — this must match `apiUrl` in the frontend's `environment.ts`.

## Project structure

```
WorkforcePortal.Domain/         # Entities, enums, exceptions
WorkforcePortal.Application/    # DTOs, services, validators, mappings
WorkforcePortal.Infrastructure/ # EF Core, repositories, auth services
WorkforcePortal.Api/            # ASP.NET Core entry point, controllers
```

## Common endpoints (base route prefix `/api`)

| Resource     | Route             |
|--------------|-------------------|
| Auth         | `/api/Auth`       |
| Employees    | `/api/Employees`  |
| Departments  | `/api/Departments`|
| Tasks        | `/api/Tasks`      |
| Audit Logs   | `/api/AuditLogs`  |
| Dashboard    | `/api/Dashboard`  |

## Troubleshooting

- **Cannot connect to SQL Server**: make sure port `1433` isn't already in use and the container is healthy (`docker logs sqlserver`).
- **Login failed for user 'sa'**: double-check the password matches exactly what was passed to `MSSQL_SA_PASSWORD` (must meet SQL Server's complexity rules — upper/lower/digit/symbol, 8+ chars).
- **Migrations fail / wrong startup project**: always pass `--project` (where `AppDbContext` lives) and `--startup-project` (where `Program.cs` and `appsettings.json` live).
