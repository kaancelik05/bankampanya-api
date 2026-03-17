# Bankampanya API .NET Runtime Decision

## Final decision
The backend project will continue with `.NET 8` instead of `.NET 9`.

## Reason
Current local SDK:
- `dotnet --version` -> `8.0.414`

To avoid unnecessary environment setup delay, the backend bootstrap and first implementation phase will proceed on `.NET 8`.

## Impact
This decision updates the earlier planning documents that originally referenced `.NET 9`.

Updated planning set:
- `/Users/kaancelik/Desktop/bankampanya/BANKAMPANYA_API_PLAN.md`
- `/Users/kaancelik/Desktop/bankampanya/BANKAMPANYA_API_DOMAIN_MODEL.md`
- `/Users/kaancelik/Desktop/bankampanya/BANKAMPANYA_API_SCAFFOLD_PLAN.md`
- `/Users/kaancelik/Desktop/bankampanya/bankampanya-api/BOOTSTRAP_STATUS.md`

## Ongoing stack
- ASP.NET Core 8 Web API
- EF Core
- Npgsql
- Supabase PostgreSQL

## Next step
Proceed with solution bootstrap in `bankampanya-api` using `net8.0` target frameworks.
