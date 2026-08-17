namespace VehicleNet.Common.Models.Search;

public sealed class VehicleSpecSearchCriteriaBuilder
{
    private int? _vehicleSpecId;
    private int? _vehicleBodyId;
    private int? _engineId;
    private string? _engine;
    private string? _manufacturer;
    private string? _model;
    private int? _generationId;
    private string? _generation;
    private int? _versionId;
    private string? _version;

    public VehicleSpecSearchCriteriaBuilder WithVehicleSpecId(int vehicleSpecId)
    {
        _vehicleSpecId = vehicleSpecId;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithEngineId(int engineId)
    {
        _engineId = engineId;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithEngine(string engine)
    {
        _engine = engine;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    public VehicleSpecSearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public VehicleSpecSearchCriteria Build() =>
        new()
        {
            VehicleSpecId = _vehicleSpecId,
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
