# CLAUDE.md - Project Instructions

## Project Overview

This is an **ASP.NET Core 8.0 backend service** for a Payroll Management System (StudioPro). It follows a layered architecture pattern with Entity Framework Core and Dapper for data access.

## Technology Stack

- **Framework**: .NET 8.0
- **ORM**: Entity Framework Core 8.0 + Dapper (hybrid approach)
- **Database**: SQL Server (MSSQL)
- **Authentication**: JWT Bearer (Web & Mobile schemes)
- **Email**: MailKit (SMTP)
- **PDF Generation**: Wkhtmltopdf, Rotativa
- **API Documentation**: Swagger/OpenAPI

## Project Structure

```
Service/
├── Controllers/Admin/       # API endpoints
├── Models/                  # EF Core entity models
├── Services/
│   ├── Admin/               # Business logic services
│   └── Helper/              # Utility services (Mail, FileUpload, Common)
├── DTO/                     # Data Transfer Objects
├── CustomModels/            # View models and custom response types
├── UnitOfWork/
│   ├── Repositories/        # Generic repository implementations
│   ├── Uow/                 # Unit of Work pattern
│   └── Exceptions/          # Custom exceptions
├── ContextHelpers/          # DbContext and Dapper context
├── Validation/              # Entity validators
├── Utils/                   # Utilities (CustomResponse, Security)
├── Upload/                  # File upload directory
└── Program.cs               # Application entry point
```

## Architecture Patterns

### Layered Architecture
1. **Presentation Layer**: Controllers (API endpoints)
2. **Business Logic Layer**: Services (IService + Implementation)
3. **Data Access Layer**: Dapper + EF Core + Unit of Work
4. **Domain Layer**: Models, DTOs, CustomModels

### Key Patterns Used
- **Repository Pattern**: `IRepository<TEntity>` with generic implementations
- **Unit of Work Pattern**: `IUowProvider` for transaction management
- **Dependency Injection**: Constructor injection throughout
- **DTO Pattern**: Services return DTOs, not domain models
- **Interface Segregation**: All services have corresponding interfaces

## Coding Conventions

### Naming Conventions
- **Controllers**: `{Feature}Controller.cs` (e.g., `RolesController.cs`)
- **Services**: `I{Feature}Service.cs` and `{Feature}Service.cs`
- **DTOs**: `{Feature}DTO.cs`
- **Models**: PascalCase singular (e.g., `Role.cs`, `User.cs`)
- **Stored Procedures**: Prefixed with `SP_` (e.g., `SP_ROLE`)

### File Organization
- Group by feature under Admin folder (e.g., `Services/Admin/Role/`)
- Keep interfaces and implementations together
- DTOs organized by domain area (`DTO/Admin/`, `DTO/Common/`)

### API Response Pattern
Always use `CustomResponse` for API responses:
```csharp
return new CustomResponse
{
    StatusCode = 200,
    Message = "Success",
    Data = result
};
```

### Service Method Pattern
Services should:
1. Accept `JObject` for flexible input
2. Use Dapper for stored procedure calls
3. Return `CustomResponse` objects
4. Be async when doing I/O operations

Example:
```csharp
public async Task<CustomResponse> Create(JObject data)
{
    var parameters = new DynamicParameters();
    parameters.Add("@Mode", "CREATE");
    parameters.Add("@Data", data.ToString());

    var result = await _dapper.ExecuteStoredProcedureAsync<ResultType>("SP_NAME", parameters);

    return new CustomResponse
    {
        StatusCode = 200,
        Data = result
    };
}
```

### Controller Pattern
```csharp
[Route("[controller]")]
[ApiController]
public class FeatureController : ControllerBase
{
    private readonly IFeatureService _service;

    public FeatureController(IFeatureService service)
    {
        _service = service;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] JObject data)
    {
        var result = await _service.Create(data);
        return Ok(result);
    }
}
```

## Database Conventions

### Stored Procedures
- Business logic is primarily in stored procedures
- Use Dapper's `DynamicParameters` for SP parameters
- Common SP modes: `CREATE`, `UPDATE`, `DELETE`, `LIST`, `GETBYID`, `CHANGESTATUS`

### Entity Properties
Standard audit fields in models:
- `CreatedBy`, `CreatedOn`
- `ModifiedBy`, `ModifiedOn`
- `IsActive` (soft delete)
- `CompanyId`, `BranchId` (multi-tenant)

## Dependency Injection Registration

Register new services in `Program.cs`:
```csharp
// Register interface and implementation
builder.Services.AddScoped<IFeatureService, FeatureService>();
```

## Adding New Features

### 1. Create Model (if needed)
Location: `Models/{EntityName}.cs`

### 2. Create DTO
Location: `DTO/Admin/{Feature}DTO.cs`

### 3. Create Service Interface
Location: `Services/Admin/{Feature}/I{Feature}Service.cs`

### 4. Create Service Implementation
Location: `Services/Admin/{Feature}/{Feature}Service.cs`

### 5. Create Controller
Location: `Controllers/Admin/{Feature}Controller.cs`

### 6. Create Validation (if needed)
Location: `Validation/Admin/{Feature}Validation.cs`

### 7. Register in DI
Add to `Program.cs`: `builder.Services.AddScoped<IFeatureService, FeatureService>();`

## Build & Run Commands

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Run with hot reload
dotnet watch run

# Run in specific environment
dotnet run --environment Development
```

## API Testing

- Swagger UI available at: `/swagger` (Development only)
- HTTP file for testing: `Service.http`

## Configuration

### Connection String
Located in `appsettings.json` under `ConnectionStrings:Service`

### Mail Settings
Located in `appsettings.json` under `MailSettings`

### File Upload Path
Located in `appsettings.json` under `paths`

## Important Notes

1. **JWT Authentication**: Two schemes exist - "Web" (requires expiration) and "Mobile" (no expiration)
2. **CORS**: Configured for `localhost:4200` (Angular frontend)
3. **Multi-tenant**: Always include `CompanyId` and `BranchId` in queries
4. **Soft Delete**: Use `IsActive` flag instead of hard deletes
5. **Async Operations**: Prefer async methods for all I/O operations

## Security Reminders

- Never commit sensitive credentials
- Use User Secrets for local development
- Use Azure Key Vault or similar for production secrets
- Validate all user inputs
- Use parameterized queries (Dapper handles this)
