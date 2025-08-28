#!/bin/bash

# PokemonTypeClash - Simple Runner Script
# Single entry point for running the application

set -e

echo "🐛 PokemonTypeClash - Type Effectiveness Analyzer"
echo "=================================================="

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ Error: .NET 9.0 SDK is not installed."
    echo "Please install .NET 9.0 SDK from: https://dotnet.microsoft.com/download"
    exit 1
fi

# Check .NET version
dotnet_version=$(dotnet --version)
echo "✅ .NET version: $dotnet_version"

# Build the application
echo "🔨 Building application..."
dotnet build PokemonTypeClash.sln --configuration Release --no-restore

# Run the application
echo "🚀 Starting PokemonTypeClash..."
echo ""

# Pass all arguments to the application
dotnet run --project src/PokemonTypeClash.Console --configuration Release "$@"
