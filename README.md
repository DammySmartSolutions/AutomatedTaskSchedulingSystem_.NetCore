# AutomatedTaskSchedulingSystem

## Overview
AutomatedTaskSchedulingSystem is a .NET 10 Razor Pages / ASP.NET Core web application for scheduling and managing tasks, employees, organizations, locations and reports. The solution uses Entity Framework Core with SQL Server (LocalDB) and ASP.NET Core Identity for authentication.

This document covers repository layout, prerequisites, setup, configuration, database/migrations, running the app, common tasks, and deployment notes.

## Quick facts
- Framework: .NET 10
- Project type: Razor Pages, with MVC-style controllers in Areas
- Auth: ASP.NET Core Identity (Areas/Identity)
- Data access: EF Core, SQL Server (LocalDB) connection strings in configuration
- Package management: per-project PackageReference (NuGet). Newtonsoft.Json upgraded to 13.0.1 in Models project.

## Repository structure
- AutomatedTaskSchedulingSystem/ — Razor Pages web project (startup, UI, wwwroot)
  - Program.cs
  - appsettings.json, appsettings.Development.json, appsettingsForDev.json
  - Areas/Identity — Identity Razor Pages and account management
  - Areas/Admin, Areas/Employ, Areas/Employee — area controllers and views
  - wwwroot — static assets (bootstrap, jquery, etc.)
- AutomatedTaskSchedulingSystem.DataAccess/ — EF Core DbContext, Migrations
  - Data/ApplicationDbContext.cs
  - Migrations/ — EF Core migrations
- AutomatedTaskSchedulingSystem.Model/ — domain models & view models
  - Model/ — entities (ApplicationUser, Task, Location, Organization, etc.)
- AutomatedTaskSchedulingSystem.Utility/ — helper utilities and shared code

## Prerequisites
- Visual Studio Community 2026 (18.6.2) or later, or dotnet 10 SDK
- .NET 10 SDK installed
- SQL Server LocalDB (or SQL Server instance)
- Optional: IIS/IIS Express for debugging

## Configuration
- appsettings.json and appsettings.Development.json control logging and runtime settings.
- appsettingsForDev.json (present in repo) contains example connection strings and email settings:
  - ConnectionStrings:
	- DefaultConnection — main DB
	- ReportConnection — report DB
	- ApplicationDbContext — used by Identity / ApplicationDbContext
  - EmailSettings: SMTP host/port/credentials used for outgoing mail (update with secure values)
  - Stimulsoft.licensekey — placeholder for reporting license key

Important: Replace sensitive values (DB credentials, email passwords, license keys) with environment variables or secret store in production.

## Build & run (Visual Studio)
1. Open solution in Visual Studio: `AutomatedTaskSchedulingSystem.sln`
2. Set `AutomatedTaskSchedulingSystem` as startup project.
3. Ensure correct environment (Development/Production) via launchSettings.
4. Run (F5) to build and start with IIS Express; or Ctrl+F5 to run without debugger.

## Build & run (dotnet CLI)
- Restore and build:
  - dotnet restore
  - dotnet build
- Run web project:
  - cd AutomatedTaskSchedulingSystem
  - dotnet run

## Database & EF Core migrations
- DbContext: AutomatedTaskSchedulingSystem.DataAccess/Data/ApplicationDbContext.cs
- Migrations folder is under AutomatedTaskSchedulingSystem.DataAccess/Migrations.
- To apply migrations:
  - dotnet ef database update --project AutomatedTaskSchedulingSystem.DataAccess --startup-project AutomatedTaskSchedulingSystem
  - Or use Visual Studio Package Manager Console:
	- Select Default project: AutomatedTaskSchedulingSystem.DataAccess
	- Update-Database -StartupProject AutomatedTaskSchedulingSystem

To add a migration:
- dotnet ef migrations add <Name> --project AutomatedTaskSchedulingSystem.DataAccess --startup-project AutomatedTaskSchedulingSystem

## Identity & Authentication
- Identity pages are scaffolded under Areas/Identity.
- ApplicationUser entity is in AutomatedTaskSchedulingSystem.Model/Model/ApplicationUser.cs.
- Identity database objects are created via migrations found in DataAccess/Migrations.

## Important source locations
- Program/Startup: AutomatedTaskSchedulingSystem/Program.cs
- Main web project file: AutomatedTaskSchedulingSystem/AutomatedTaskSchedulingSystem.csproj
- DataAccess project file: AutomatedTaskSchedulingSystem.DataAccess/AutomatedTaskSchedulingSystem.DataAccess.csproj
- Models project file: AutomatedTaskSchedulingSystem.Model/AutomatedTaskSchedulingSystem.Models.csproj
- Utility project file: AutomatedTaskSchedulingSystem.Utility/AutomatedTaskSchedulingSystem.Utility.csproj

## NuGet & package vulnerabilities
- The solution uses NuGet PackageReference. A recent update added Newtonsoft.Json v13.0.1 to the Models project to address a reported vulnerability.
- If you centralize package versions later, prefer Directory.Packages.props (Central Package Management) and remove version attributes from per-project PackageReference entries.

## Logging and diagnostics
- Default logging levels defined in appsettings*.json.
- For production, configure structured logging (Serilog or built-in providers) and secure log sinks.

## Testing
- No dedicated test project included in the repo root. Add xUnit / NUnit projects to implement unit and integration tests.
- For EF Core integration tests, consider using an in-memory provider or a disposable real database.

## Deployment
- Deploy like any ASP.NET Core app:
  - Publish from Visual Studio (right-click project -> Publish)
  - Or dotnet publish -c Release -o ./publish
- Configure production connection strings and secrets via environment variables, Azure App Configuration, or user-secrets during development.

## Common tasks & commands
- Restore: dotnet restore
- Build: dotnet build
- Run web: dotnet run --project AutomatedTaskSchedulingSystem
- Add migration: dotnet ef migrations add <Name> --project AutomatedTaskSchedulingSystem.DataAccess --startup-project AutomatedTaskSchedulingSystem
- Update DB: dotnet ef database update --project AutomatedTaskSchedulingSystem.DataAccess --startup-project AutomatedTaskSchedulingSystem

## Troubleshooting
- EF Core errors: confirm connection string and SQL Server instance; run Update-Database and inspect migration snapshots.
- Identity issues: ensure ApplicationDbContext connection string is correct and migrations applied.
- Static files or client assets not loading: ensure wwwroot content is present and UseStaticFiles is configured in Program.cs.

## Security recommendations
- Do not store secrets in appsettings files in source control; use environment variables, user-secrets, or an Azure Key Vault.
- Keep NuGet packages up to date; use tools to scan for vulnerabilities regularly.
- Enforce HTTPS in production and set HSTS.

## Contribution guide
- Create an issue describing the change.
- Open a feature branch from main.
- Add clear commit messages and unit tests where relevant.
- Create a PR, ensure CI builds successfully (build + tests), and request review.

## Useful references
- .NET 10 docs: https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10
- EF Core docs: https://learn.microsoft.com/ef/core/
- ASP.NET Core Identity: https://learn.microsoft.com/aspnet/core/security/authentication/identity


If you want, I can:
- Add CONTRIBUTING.md or a CI pipeline template.
- Produce a short architecture diagram (textual) or list of public APIs/endpoints.
