$ErrorActionPreference = "Stop"

Write-Host "Creating Solution..."
dotnet new sln -n WorkforcePortal --force

Write-Host "Creating Projects..."
dotnet new classlib -n WorkforcePortal.Domain --force
dotnet new classlib -n WorkforcePortal.Application --force
dotnet new classlib -n WorkforcePortal.Infrastructure --force

Write-Host "Adding Projects to Solution..."
dotnet sln add WorkforcePortal.Domain/WorkforcePortal.Domain.csproj
dotnet sln add WorkforcePortal.Application/WorkforcePortal.Application.csproj
dotnet sln add WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj
dotnet sln add workspace_backend.csproj

Write-Host "Adding References..."
dotnet add WorkforcePortal.Application/WorkforcePortal.Application.csproj reference WorkforcePortal.Domain/WorkforcePortal.Domain.csproj
dotnet add WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj reference WorkforcePortal.Application/WorkforcePortal.Application.csproj
dotnet add workspace_backend.csproj reference WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj
dotnet add workspace_backend.csproj reference WorkforcePortal.Application/WorkforcePortal.Application.csproj

Write-Host "Adding NuGet Packages..."
# API
dotnet add workspace_backend.csproj package Microsoft.AspNetCore.Authentication.JwtBearer -v 10.0.0
dotnet add workspace_backend.csproj package Serilog.AspNetCore -v 9.0.0
dotnet add workspace_backend.csproj package Swashbuckle.AspNetCore -v 7.2.0

# Infrastructure
dotnet add WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer -v 9.0.0
dotnet add WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools -v 9.0.0
dotnet add WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj package System.IdentityModel.Tokens.Jwt -v 8.3.0
dotnet add WorkforcePortal.Infrastructure/WorkforcePortal.Infrastructure.csproj package BCrypt.Net-Next -v 4.0.3

# Application
dotnet add WorkforcePortal.Application/WorkforcePortal.Application.csproj package FluentValidation.AspNetCore -v 11.3.0
dotnet add WorkforcePortal.Application/WorkforcePortal.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection -v 12.0.1

Write-Host "Setup Complete!"
