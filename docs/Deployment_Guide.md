# Deployment Guide - PokemonTypeClash

## Overview

This guide provides comprehensive instructions for deploying the PokemonTypeClash application across different environments and platforms.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Local Development Deployment](#local-development-deployment)
3. [Production Deployment](#production-deployment)
4. [Monitoring and Maintenance](#monitoring-and-maintenance)

## Prerequisites

### System Requirements

- **Operating System**: Windows 10+, macOS 10.15+, or Linux (Ubuntu 20.04+)
- **.NET Runtime**: .NET 9.0 Runtime (for development builds)
- **Memory**: Minimum 512MB RAM, Recommended 2GB+
- **Storage**: Minimum 100MB free space
- **Network**: Internet connection for PokéAPI access

### Required Software

- **.NET 9.0 SDK** (for development builds)
- **Git** (for source code management)

## Local Development Deployment

### Option 1: Direct .NET Execution

1. **Clone the Repository**
   ```bash
   git clone https://github.com/ryan-hebert/PokemonTypeClash.git
   cd PokemonTypeClash
   ```

2. **Build the Application**
   ```bash
   dotnet build
   ```

3. **Run the Application**
   ```bash
   dotnet run --project src/PokemonTypeClash.Console
   ```

### Option 2: Using Run Scripts

1. **Clone the Repository**
   ```bash
   git clone https://github.com/ryan-hebert/PokemonTypeClash.git
   cd PokemonTypeClash
   ```

2. **Run with Platform-Specific Scripts**
   ```bash
   # Windows
   run.bat
   
   # macOS/Linux
   chmod +x run.sh
   ./run.sh
   ```

### Option 3: Published Application

1. **Publish the Application**
   ```bash
   # For current platform
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish
   
   # For specific platforms
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/win-x64 --self-contained -r win-x64 -p:PublishSingleFile=true
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/linux-x64 --self-contained -r linux-x64 -p:PublishSingleFile=true
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/osx-x64 --self-contained -r osx-x64 -p:PublishSingleFile=true
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/osx-arm64 --self-contained -r osx-arm64 -p:PublishSingleFile=true
   ```

2. **Run the Published Application**
   ```bash
   # Windows
   ./publish/win-x64/PokemonTypeClash.Console.exe
   
   # Linux/macOS
   ./publish/linux-x64/PokemonTypeClash.Console
   ./publish/osx-x64/PokemonTypeClash.Console
   ./publish/osx-arm64/PokemonTypeClash.Console
   ```

## Production Deployment

### Environment Configuration

1. **Production appsettings.json**
   ```json
   {
     "PokeApi": {
       "BaseUrl": "https://pokeapi.co/api/v2/",
       "TimeoutSeconds": 60,
       "MaxRetries": 5,
       "CacheDurationMinutes": 120
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft": "Warning",
         "System": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```

2. **Environment Variables**
   ```bash
   export ASPNETCORE_ENVIRONMENT=Production
   export PokeApi__TimeoutSeconds=60
   export PokeApi__MaxRetries=5
   export PokeApi__CacheDurationMinutes=120
   ```

### Security Considerations

1. **Network Security**
   - Use HTTPS for all external communications
   - Implement proper firewall rules
   - Restrict access to necessary ports only

2. **Application Security**
   - Validate all input data
   - Implement proper error handling
   - Use secure configuration management
   - Enable logging and monitoring

### High Availability Setup

1. **Load Balancer Configuration**
   ```yaml
   # Nginx configuration example
   upstream pokemontypeclash {
     server 10.0.1.10:8080;
     server 10.0.1.11:8080;
     server 10.0.1.12:8080;
   }
   
   server {
     listen 80;
     server_name pokemontypeclash.example.com;
     
     location / {
       proxy_pass http://pokemontypeclash;
       proxy_set_header Host $host;
       proxy_set_header X-Real-IP $remote_addr;
     }
   }
   ```

2. **Health Check Configuration**
   ```bash
   # Health check endpoint
   curl -f http://localhost:8080/health || exit 1
   ```

## Monitoring and Maintenance

### Logging Configuration

1. **Structured Logging**
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft": "Warning",
         "System": "Warning"
       },
       "Console": {
         "FormatterName": "json",
         "FormatterOptions": {
           "IncludeScopes": true,
           "TimestampFormat": "yyyy-MM-dd HH:mm:ss "
         }
       }
     }
   }
   ```

2. **Log Aggregation**
   - Use ELK Stack (Elasticsearch, Logstash, Kibana)
   - Implement centralized logging
   - Set up log retention policies

### Performance Monitoring

1. **Application Metrics**
   - Response times
   - Error rates
   - Memory usage
   - CPU utilization

2. **Infrastructure Metrics**
   - Container health
   - Network performance
   - Disk usage
   - Resource utilization

### Backup and Recovery

1. **Configuration Backup**
   ```bash
   # Backup configuration
   cp appsettings.json appsettings.json.backup
   
   # Restore configuration
   cp appsettings.json.backup appsettings.json
   ```

### Update Procedures

1. **Zero-Downtime Deployment**
   ```bash
   # Blue-green deployment
   # Deploy new version to green environment
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./green
   
   # Switch traffic to green
   # Stop blue environment
   # Rename green to blue
   ```

2. **Rollback Procedure**
   ```bash
   # Rollback to previous version
   # Restore from backup
   cp appsettings.json.backup appsettings.json
   ```

## Troubleshooting

### Common Issues

1. **Application Won't Start**
   ```bash
   # Check logs
   dotnet run --project src/PokemonTypeClash.Console --verbosity normal
   
   # Check configuration
   cat appsettings.json
   
   # Check permissions
   ls -la src/PokemonTypeClash.Console/
   ```

2. **API Connection Issues**
   ```bash
   # Test API connectivity
   curl -v https://pokeapi.co/api/v2/pokemon/pikachu
   
   # Check network configuration
   ping pokeapi.co
   ```

3. **Performance Issues**
   ```bash
   # Check resource usage
   top
   
   # Check memory usage
   free -h
   ```

4. **macOS Security Issues**
   ```bash
   # Remove quarantine attribute
   xattr -d com.apple.quarantine /path/to/PokemonTypeClash.Console
   
   # Set executable permissions
   chmod +x PokemonTypeClash.Console
   ```

### Support and Resources

- **Documentation**: [Project README](README.md)
- **Issues**: [GitHub Issues](https://github.com/ryan-hebert/PokemonTypeClash/issues)
- **Discussions**: [GitHub Discussions](https://github.com/ryan-hebert/PokemonTypeClash/discussions)
- **Releases**: [GitHub Releases](https://github.com/ryan-hebert/PokemonTypeClash/releases)

## Conclusion

This deployment guide provides comprehensive instructions for deploying PokemonTypeClash across various environments. Follow the security best practices and monitoring recommendations to ensure a robust production deployment.

For additional support or questions, please refer to the project documentation or create an issue on GitHub.
