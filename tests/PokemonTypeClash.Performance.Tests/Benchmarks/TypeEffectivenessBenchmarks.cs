using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonTypeClash.Application.Services;
using PokemonTypeClash.Core.Models;
using PokemonTypeClash.Core.Enums;
using PokemonTypeClash.Core.Interfaces;

namespace PokemonTypeClash.Performance.Tests.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class TypeEffectivenessBenchmarks
{
    private TypeEffectivenessService _service = null!;
    private PokemonType _electricType = null!;
    private PokemonType _waterType = null!;
    private PokemonType _fireType = null!;
    private PokemonType _grassType = null!;
    private PokemonType _groundType = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Create mock logger and type data service for benchmarks
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<TypeEffectivenessService>();
        
        // Create a simple mock type data service
        var mockTypeDataService = new Mock<ITypeDataService>();
        mockTypeDataService.Setup(x => x.GetAllTypesAsync())
            .ReturnsAsync(new List<PokemonType>());
        
        _service = new TypeEffectivenessService(logger, mockTypeDataService.Object);
        
        // Create test types with relations
        _electricType = new PokemonType
        {
            Id = 13,
            Name = "electric",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { _waterType, new PokemonType { Name = "flying" } },
                HalfDamageTo = new List<PokemonType> { _electricType, _grassType, new PokemonType { Name = "dragon" } },
                NoDamageTo = new List<PokemonType> { _groundType },
                DoubleDamageFrom = new List<PokemonType> { _groundType },
                HalfDamageFrom = new List<PokemonType> { new PokemonType { Name = "flying" }, new PokemonType { Name = "steel" }, _electricType },
                NoDamageFrom = new List<PokemonType>()
            }
        };

        _waterType = new PokemonType
        {
            Id = 11,
            Name = "water",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { _fireType, _groundType },
                HalfDamageTo = new List<PokemonType> { _waterType, _grassType, new PokemonType { Name = "dragon" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { _electricType, _grassType },
                HalfDamageFrom = new List<PokemonType> { _fireType, _waterType, new PokemonType { Name = "ice" }, new PokemonType { Name = "steel" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };

        _fireType = new PokemonType
        {
            Id = 10,
            Name = "fire",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { _grassType, new PokemonType { Name = "ice" }, new PokemonType { Name = "bug" }, new PokemonType { Name = "steel" } },
                HalfDamageTo = new List<PokemonType> { _fireType, _waterType, new PokemonType { Name = "rock" }, new PokemonType { Name = "dragon" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { _waterType, _groundType, new PokemonType { Name = "rock" } },
                HalfDamageFrom = new List<PokemonType> { _fireType, _grassType, new PokemonType { Name = "ice" }, new PokemonType { Name = "bug" }, new PokemonType { Name = "steel" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };

        _grassType = new PokemonType
        {
            Id = 12,
            Name = "grass",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { _waterType, _groundType, new PokemonType { Name = "rock" } },
                HalfDamageTo = new List<PokemonType> { _fireType, _grassType, new PokemonType { Name = "poison" }, new PokemonType { Name = "flying" }, new PokemonType { Name = "bug" }, new PokemonType { Name = "dragon" }, new PokemonType { Name = "steel" } },
                NoDamageTo = new List<PokemonType>(),
                DoubleDamageFrom = new List<PokemonType> { _fireType, new PokemonType { Name = "ice" }, new PokemonType { Name = "poison" }, new PokemonType { Name = "flying" }, new PokemonType { Name = "bug" } },
                HalfDamageFrom = new List<PokemonType> { _waterType, _electricType, _grassType, new PokemonType { Name = "ground" } },
                NoDamageFrom = new List<PokemonType>()
            }
        };

        _groundType = new PokemonType
        {
            Id = 5,
            Name = "ground",
            Relations = new TypeRelations
            {
                DoubleDamageTo = new List<PokemonType> { _fireType, _electricType, new PokemonType { Name = "poison" }, new PokemonType { Name = "rock" }, new PokemonType { Name = "steel" } },
                HalfDamageTo = new List<PokemonType> { _grassType, new PokemonType { Name = "bug" } },
                NoDamageTo = new List<PokemonType> { new PokemonType { Name = "flying" } },
                DoubleDamageFrom = new List<PokemonType> { _waterType, _grassType, new PokemonType { Name = "ice" } },
                HalfDamageFrom = new List<PokemonType> { new PokemonType { Name = "poison" }, new PokemonType { Name = "rock" } },
                NoDamageFrom = new List<PokemonType> { _electricType }
            }
        };
    }

    [Benchmark]
    public TypeEffectiveness ElectricVsWater()
    {
        return _service.CalculateEffectiveness(_electricType, _waterType);
    }

    [Benchmark]
    public TypeEffectiveness WaterVsFire()
    {
        return _service.CalculateEffectiveness(_waterType, _fireType);
    }

    [Benchmark]
    public TypeEffectiveness FireVsGrass()
    {
        return _service.CalculateEffectiveness(_fireType, _grassType);
    }

    [Benchmark]
    public TypeEffectiveness GrassVsWater()
    {
        return _service.CalculateEffectiveness(_grassType, _waterType);
    }

    [Benchmark]
    public TypeEffectiveness GroundVsElectric()
    {
        return _service.CalculateEffectiveness(_groundType, _electricType);
    }

    [Benchmark]
    public TypeEffectiveness ElectricVsGround()
    {
        return _service.CalculateEffectiveness(_electricType, _groundType);
    }
}
