@echo off
REM PokemonTypeClash - Windows Runner Script
REM Single entry point for running the application on Windows

echo.
echo ==================================================
echo PokemonTypeClash - Type Effectiveness Analyzer
echo ==================================================

REM Check if .NET is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET 9.0 SDK is not installed.
    echo Please install .NET 9.0 SDK from: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

REM Check .NET version
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo .NET version: %DOTNET_VERSION%

REM Build the application
echo.
echo Building application...
dotnet build PokemonTypeClash.sln --configuration Release --no-restore

REM Run the application
echo.
echo Starting PokemonTypeClash...
echo.

REM Pass all arguments to the application
dotnet run --project src/PokemonTypeClash.Console --configuration Release %*
