## Important environment note
The local machine currently reports:
- `dotnet --version` -> `8.0.414`

## Decision
The backend implementation will proceed with `.NET 8` to match the currently installed SDK.

## Planned scaffold commands
```bash
cd /Users/kaancelik/Desktop/bankampanya/bankampanya-api

dotnet new sln -n Bankampanya
mkdir -p src tests

cd src
dotnet new webapi -n Bankampanya.Api --framework net8.0
dotnet new classlib -n Bankampanya.Application --framework net8.0
dotnet new classlib -n Bankampanya.Domain --framework net8.0
dotnet new classlib -n Bankampanya.Infrastructure --framework net8.0

cd ../tests
dotnet new xunit -n Bankampanya.Api.Tests --framework net8.0
dotnet new xunit -n Bankampanya.Application.Tests --framework net8.0
```

Then add references and packages.
