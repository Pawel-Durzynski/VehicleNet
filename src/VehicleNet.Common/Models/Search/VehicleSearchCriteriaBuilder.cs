namespace VehicleNet.Common.Models.Search;

/// <summary>Builds vehicle search criteria.</summary>
public sealed class VehicleSearchCriteriaBuilder
{
    private int? _engineId;
    private string? _engine;
    private string? _manufacturer;
    private int? _vehicleBodyEngineId;
    private int? _vehicleBodyId;
    private string? _model;
    private int? _generationId;
    private string? _generation;
    private int? _versionId;
    private string? _version;

    /// <summary>Sets the engine identifier filter.</summary>
    public VehicleSearchCriteriaBuilder WithEngineId(int engineId)
    {
        _engineId = engineId;
        return this;
    }

    /// <summary>Sets the engine name filter.</summary>
    public VehicleSearchCriteriaBuilder WithEngine(string engine)
    {
        _engine = engine;
        return this;
    }

    /// <summary>Sets the manufacturer name filter.</summary>
    public VehicleSearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    /// <summary>Sets the body-engine identifier filter.</summary>
    public VehicleSearchCriteriaBuilder WithVehicleBodyEngineId(int vehicleBodyEngineId)
    {
        _vehicleBodyEngineId = vehicleBodyEngineId;
        return this;
    }

    /// <summary>Sets the vehicle body identifier filter.</summary>
    public VehicleSearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    /// <summary>Sets the model name filter.</summary>
    public VehicleSearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    /// <summary>Sets the generation identifier filter.</summary>
    public VehicleSearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    /// <summary>Sets the generation name filter.</summary>
    public VehicleSearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    /// <summary>Sets the version identifier filter.</summary>
    public VehicleSearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    /// <summary>Sets the version name filter.</summary>
    public VehicleSearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    /// <summary>Builds the configured search criteria.</summary>
    public VehicleSearchCriteria Build() =>
        new()
        {
            EngineId = _engineId,
            Engine = _engine,
            Manufacturer = _manufacturer,
            VehicleBodyEngineId = _vehicleBodyEngineId,
            VehicleBodyId = _vehicleBodyId,
            Model = _model,
            GenerationId = _generationId,
            Generation = _generation,
            VersionId = _versionId,
            Version = _version,
        };
}
