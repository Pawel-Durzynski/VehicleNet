namespace VehicleNet.Common.Models.Search;

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

    public VehicleBodyEngineSearchCriteriaBuilder WithVehicleBodyEngineId(int vehicleBodyEngineId)
    {
        _vehicleBodyEngineId = vehicleBodyEngineId;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithEngineId(int engineId)
    {
        _engineId = engineId;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithEngine(string engine)
    {
        _engine = engine;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    public VehicleBodyEngineSearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

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
