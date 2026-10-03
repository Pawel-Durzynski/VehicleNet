namespace VehicleNet.Common.Models.Search;

/// <summary>Builds vehicle body-engine search criteria.</summary>
public sealed class VehicleBodyEngineSearchCriteriaBuilder
{
    private int? _vehicleBodyEngineId;
    private int? _vehicleBodyId;
    private int? _engineId;
    private string? _engine;
    private string? _manufacturer;
    private string? _model;
    private int? _generationId;
    private string? _generation;
    private int? _versionId;
    private string? _version;

    /// <summary>Sets the body-engine identifier filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithVehicleBodyEngineId(int vehicleBodyEngineId)
    {
        _vehicleBodyEngineId = vehicleBodyEngineId;
        return this;
    }

    /// <summary>Sets the vehicle body identifier filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    /// <summary>Sets the engine identifier filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithEngineId(int engineId)
    {
        _engineId = engineId;
        return this;
    }

    /// <summary>Sets the engine name filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithEngine(string engine)
    {
        _engine = engine;
        return this;
    }

    /// <summary>Sets the manufacturer name filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    /// <summary>Sets the model name filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    /// <summary>Sets the generation identifier filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    /// <summary>Sets the generation name filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    /// <summary>Sets the version identifier filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    /// <summary>Sets the version name filter.</summary>
    public VehicleBodyEngineSearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    /// <summary>Builds the configured search criteria.</summary>
    public VehicleBodyEngineSearchCriteria Build() =>
        new()
        {
            VehicleBodyEngineId = _vehicleBodyEngineId,
            VehicleBodyId = _vehicleBodyId,
            EngineId = _engineId,
            Engine = _engine,
            Manufacturer = _manufacturer,
            Model = _model,
            GenerationId = _generationId,
            Generation = _generation,
            VersionId = _versionId,
            Version = _version,
        };
}
