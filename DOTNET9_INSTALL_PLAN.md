# Bankampanya API .NET 9 Install Plan

## Current machine status
- `dotnet --version`: `8.0.414`
- Homebrew: available
- Architecture: `arm64`

## Recommended install path
Use Homebrew to install the .NET 9 SDK on this Mac.

## Recommended commands
```bash
brew update
brew install --cask dotnet-sdk

dotnet --list-sdks
dotnet --version
```

## Verification target
After install, one of the listed SDKs should be `9.x.x`.

## Then continue with backend bootstrap
```bash
cd /Users/kaancelik/Desktop/bankampanya/bankampanya-api

dotnet new sln -n Bankampanya
mkdir -p src tests

cd src
dotnet new webapi -n Bankampanya.Api --framework net9.0
dotnet new classlib -n Bankampanya.Application --framework net9.0
dotnet new classlib -n Bankampanya.Domain --framework net9.0
dotnet new classlib -n Bankampanya.Infrastructure --framework net9.0

cd ../tests
dotnet new xunit -n Bankampanya.Api.Tests --framework net9.0
dotnet new xunit -n Bankampanya.Application.Tests --framework net9.0
```
