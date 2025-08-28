# Deployment Guide - PokemonTypeClash

## Overview

This guide provides comprehensive instructions for deploying the PokemonTypeClash application across different environments and platforms.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Local Development Deployment](#local-development-deployment)
3. [Docker Deployment](#docker-deployment)
4. [Cloud Deployment](#cloud-deployment)
5. [Production Deployment](#production-deployment)
6. [Monitoring and Maintenance](#monitoring-and-maintenance)

## Prerequisites

### System Requirements

- **Operating System**: Windows 10+, macOS 10.15+, or Linux (Ubuntu 20.04+)
- **.NET Runtime**: .NET 9.0 Runtime
- **Memory**: Minimum 512MB RAM, Recommended 2GB+
- **Storage**: Minimum 100MB free space
- **Network**: Internet connection for PokéAPI access

### Required Software

- **.NET 9.0 SDK** (for development builds)
- **Docker** (for containerized deployment)
- **Git** (for source code management)

## Local Development Deployment

### Option 1: Direct .NET Execution

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/PokemonTypeClash.git
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

### Option 2: Published Application

1. **Publish the Application**
   ```bash
   # For current platform
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish
   
   # For specific platform
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/win-x64 --self-contained -r win-x64
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/linux-x64 --self-contained -r linux-x64
   dotnet publish src/PokemonTypeClash.Console -c Release -o ./publish/osx-x64 --self-contained -r osx-x64
   ```

2. **Run the Published Application**
   ```bash
   # Windows
   ./publish/win-x64/PokemonTypeClash.Console.exe
   
   # Linux/macOS
   ./publish/linux-x64/PokemonTypeClash.Console
   ./publish/osx-x64/PokemonTypeClash.Console
   ```

## Docker Deployment

### Local Docker Deployment

1. **Build Docker Image**
   ```bash
   docker build -t pokemontypeclash .
   ```

2. **Run Container**
   ```bash
   # Interactive mode
   docker run -it pokemontypeclash
   
   # Background mode
   docker run -d --name pokemontypeclash-app pokemontypeclash
   
   # With custom configuration
   docker run -it -v $(pwd)/appsettings.json:/app/appsettings.json:ro pokemontypeclash
   ```

### Docker Compose Deployment

1. **Start All Services**
   ```bash
   docker-compose up -d
   ```

2. **View Logs**
   ```bash
   docker-compose logs -f pokemontypeclash
   ```

3. **Stop Services**
   ```bash
   docker-compose down
   ```

### Production Docker Deployment

1. **Build Production Image**
   ```bash
   docker build -t pokemontypeclash:latest .
   ```

2. **Run with Production Settings**
   ```bash
   docker run -d \
     --name pokemontypeclash-prod \
     -e ASPNETCORE_ENVIRONMENT=Production \
     -e PokeApi__TimeoutSeconds=60 \
     -e PokeApi__MaxRetries=5 \
     -v /var/log/pokemontypeclash:/app/logs \
     --restart unless-stopped \
     pokemontypeclash:latest
   ```

## Cloud Deployment

### Azure Container Instances (ACI)

1. **Deploy to ACI**
   ```bash
   az container create \
     --resource-group myResourceGroup \
     --name pokemontypeclash \
     --image yourregistry.azurecr.io/pokemontypeclash:latest \
     --dns-name-label pokemontypeclash \
     --ports 8080 \
     --environment-variables ASPNETCORE_ENVIRONMENT=Production
   ```

2. **Scale ACI**
   ```bash
   az container update \
     --resource-group myResourceGroup \
     --name pokemontypeclash \
     --cpu 2 \
     --memory 4
   ```

### Azure Kubernetes Service (AKS)

1. **Create Kubernetes Deployment**
   ```yaml
   apiVersion: apps/v1
   kind: Deployment
   metadata:
     name: pokemontypeclash
   spec:
     replicas: 3
     selector:
       matchLabels:
         app: pokemontypeclash
     template:
       metadata:
         labels:
           app: pokemontypeclash
       spec:
         containers:
         - name: pokemontypeclash
           image: yourregistry.azurecr.io/pokemontypeclash:latest
           ports:
           - containerPort: 8080
           env:
           - name: ASPNETCORE_ENVIRONMENT
             value: "Production"
           resources:
             requests:
               memory: "256Mi"
               cpu: "250m"
             limits:
               memory: "512Mi"
               cpu: "500m"
   ```

2. **Apply Deployment**
   ```bash
   kubectl apply -f deployment.yaml
   ```

### AWS ECS (Elastic Container Service)

1. **Create ECS Task Definition**
   ```json
   {
     "family": "pokemontypeclash",
     "networkMode": "awsvpc",
     "requiresCompatibilities": ["FARGATE"],
     "cpu": "256",
     "memory": "512",
     "executionRoleArn": "arn:aws:iam::123456789012:role/ecsTaskExecutionRole",
     "containerDefinitions": [
       {
         "name": "pokemontypeclash",
         "image": "yourregistry/pokemontypeclash:latest",
         "portMappings": [
           {
             "containerPort": 8080,
             "protocol": "tcp"
           }
         ],
         "environment": [
           {
             "name": "ASPNETCORE_ENVIRONMENT",
             "value": "Production"
           }
         ],
         "logConfiguration": {
           "logDriver": "awslogs",
           "options": {
             "awslogs-group": "/ecs/pokemontypeclash",
             "awslogs-region": "us-east-1",
             "awslogs-stream-prefix": "ecs"
           }
         }
       }
     ]
   }
   ```

2. **Deploy to ECS**
   ```bash
   aws ecs register-task-definition --cli-input-json file://task-definition.json
   aws ecs create-service --cluster my-cluster --service-name pokemontypeclash --task-definition pokemontypeclash:1
   ```

### Google Cloud Run

1. **Deploy to Cloud Run**
   ```bash
   gcloud run deploy pokemontypeclash \
     --image gcr.io/your-project/pokemontypeclash:latest \
     --platform managed \
     --region us-central1 \
     --allow-unauthenticated \
     --memory 512Mi \
     --cpu 1 \
     --set-env-vars ASPNETCORE_ENVIRONMENT=Production
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

2. **Container Security**
   - Run containers as non-root user
   - Use minimal base images
   - Regularly update base images
   - Scan images for vulnerabilities

3. **Application Security**
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

2. **Data Backup** (for future database features)
   ```bash
   # PostgreSQL backup
   pg_dump pokemontypeclash > backup.sql
   
   # Redis backup
   redis-cli BGSAVE
   ```

### Update Procedures

1. **Zero-Downtime Deployment**
   ```bash
   # Blue-green deployment
   # Deploy new version to green environment
   docker run -d --name pokemontypeclash-green pokemontypeclash:new
   
   # Switch traffic to green
   docker stop pokemontypeclash-blue
   docker rm pokemontypeclash-blue
   
   # Rename green to blue
   docker rename pokemontypeclash-green pokemontypeclash-blue
   ```

2. **Rollback Procedure**
   ```bash
   # Rollback to previous version
   docker stop pokemontypeclash-blue
   docker run -d --name pokemontypeclash-blue pokemontypeclash:previous
   ```

## Troubleshooting

### Common Issues

1. **Application Won't Start**
   ```bash
   # Check logs
   docker logs pokemontypeclash
   
   # Check configuration
   docker exec pokemontypeclash cat /app/appsettings.json
   
   # Check permissions
   docker exec pokemontypeclash ls -la /app
   ```

2. **API Connection Issues**
   ```bash
   # Test API connectivity
   curl -v https://pokeapi.co/api/v2/pokemon/pikachu
   
   # Check network configuration
   docker exec pokemontypeclash ping pokeapi.co
   ```

3. **Performance Issues**
   ```bash
   # Check resource usage
   docker stats pokemontypeclash
   
   # Check memory usage
   docker exec pokemontypeclash free -h
   ```

### Support and Resources

- **Documentation**: [Project README](README.md)
- **Issues**: [GitHub Issues](https://github.com/yourusername/PokemonTypeClash/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/PokemonTypeClash/discussions)
- **Wiki**: [Project Wiki](https://github.com/yourusername/PokemonTypeClash/wiki)

## Conclusion

This deployment guide provides comprehensive instructions for deploying PokemonTypeClash across various environments. Follow the security best practices and monitoring recommendations to ensure a robust production deployment.

For additional support or questions, please refer to the project documentation or create an issue on GitHub.
