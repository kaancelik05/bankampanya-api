# Bankampanya API Database Setup Notes

## Current setup state
The API currently targets `.NET 8` and uses:
- EF Core 8
- Npgsql
- PostgreSQL-compatible configuration

## Configuration added
The following have been prepared:
- `ConnectionStrings:Default`
- `Supabase` options section
- `AppDbContextFactory` for EF design-time operations
- migrations assembly wiring in Infrastructure

## Important note
Current `appsettings*.json` files contain local placeholder PostgreSQL values only.
They are not the real Supabase credentials.

## Before first real migration against Supabase
Replace the connection string with the actual Supabase PostgreSQL connection string using secrets/environment variables.

Recommended secure values to provide later:
- `ConnectionStrings__Default`
- `Supabase__Url`
- `Supabase__AnonKey`
- `Supabase__ServiceRoleKey`

## Safe next step
The project is now ready for:
- local migration generation
- later connection to Supabase PostgreSQL once real credentials are supplied
