# Bankampanya API

ASP.NET Core / .NET 8 backend for the Bankampanya product ecosystem.

## Scope
This service powers:
- mobile read APIs
- mobile mutation APIs
- mobile authentication APIs
- admin content management APIs

## Tech stack
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL / Supabase
- FluentValidation
- JWT authentication

## Solution structure
- `src/Bankampanya.Api` — API host, controllers, middleware, auth wiring
- `src/Bankampanya.Application` — services, DTOs, validation, interfaces
- `src/Bankampanya.Domain` — entities and enums
- `src/Bankampanya.Infrastructure` — EF Core, repositories, security, persistence
- `tests/Bankampanya.Api.Tests` — API/integration-style tests with fakes

## Implemented areas
### Admin APIs
- campaigns
- credits
- notifications
- tracking
- assistant prompts
- dashboard summary

### Mobile APIs
- campaigns
- credits
- notifications
- tracking
- earnings
- profile
- wallet
- assistant suggestions

### Mobile auth
- `POST /api/mobile/auth/login`
- `POST /api/mobile/auth/register`
- `POST /api/mobile/auth/password-reset`
- `GET /api/mobile/auth/me`
- `POST /api/mobile/auth/refresh`
- `POST /api/mobile/auth/logout`

## Auth status
Auth is implemented with:
- persisted users (`app_users`)
- password hashing
- signed JWT access tokens
- `/me` authorization
- refresh token rotation/revocation
- DB-backed refresh token persistence via `refresh_token_grants`

## Local development
### Requirements
- .NET SDK 8
- PostgreSQL-compatible connection string
- optional Supabase project for hosted runtime

### Environment
Create:
- `src/Bankampanya.Api/.env`

Use:
- `src/Bankampanya.Api/.env.example`

Do not commit real secrets.

### Run locally
```bash
cd src/Bankampanya.Api
set -a && source ./.env && set +a
ASPNETCORE_URLS=http://0.0.0.0:5263 ASPNETCORE_HTTPS_PORT=5263 dotnet run
```

API default local URL:
- `http://localhost:5263`

## Migrations
Important SQL scripts:
- `supabase-migration.sql`
- `app-users-migration.sql`
- `refresh-token-grants-migration.sql`

In this project, direct migration execution may be unreliable from the current environment.
Practical workflow:
1. generate SQL
2. open Supabase SQL Editor
3. apply the script manually

## Tests
Run backend tests:
```bash
cd /Users/kaancelik/Desktop/bankampanya/bankampanya-api
dotnet test tests/Bankampanya.Api.Tests/Bankampanya.Api.Tests.csproj
```

Current status:
- `32/32` tests passing

## Notes
- Demo reseed is controlled by `DemoData:ReseedOnStartup`
- development keeps reseed on for deterministic smoke
- targeted `AsNoTracking()` is preferred over global no-tracking defaults
- invalid login / revoked refresh reuse returns `401`
