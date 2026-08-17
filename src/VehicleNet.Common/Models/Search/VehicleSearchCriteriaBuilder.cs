namespace VehicleNet.Common.Models.Search;

public sealed class VehicleSearchCriteriaBuilder
{
    private int? _engineId;
    private string? _engine;
    private string? _manufacturer;
    private int? _vehicleSpecId;
    private int? _vehicleBodyId;
    private string? _model;
    private int? _generationId;
    private string? _generation;
    private int? _versionId;
    private string? _version;

    public VehicleSearchCriteriaBuilder WithEngineId(int engineId)
    {
        _engineId = engineId;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithEngine(string engine)
    {
        _engine = engine;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithVehicleSpecId(int vehicleSpecId)
    {
        _vehicleSpecId = vehicleSpecId;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    public VehicleSearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public VehicleSearchCriteria Build() =>
        new()
        {
            EngineId = _engineId,
            Engine = _engine,
            Manufacturer = _manufacturer,
            VehicleSpecId = _vehicleSpecId,
            VehicleBodyId = _vehicleBodyId,
            Model = _model,
            GenerationId = _generationId,
            Generation = _generation,
            VersionId = _versionId,
            Version = _version,
        };
}
