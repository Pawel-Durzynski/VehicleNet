namespace VehicleNet.Common.Models.Search;

public sealed class VehicleBodySearchCriteriaBuilder
{
    private int? _vehicleBodyId;
    private string? _manufacturer;
    private string? _model;
    private int? _generationId;
    private string? _generation;
    private int? _versionId;
    private string? _version;

    public VehicleBodySearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    public VehicleBodySearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    public VehicleBodySearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    public VehicleBodySearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    public VehicleBodySearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    public VehicleBodySearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    public VehicleBodySearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    public VehicleBodySearchCriteria Build() =>
        new()
        {
            VehicleBodyId = _vehicleBodyId,
            Manufacturer = _manufacturer,
            Model = _model,
            GenerationId = _generationId,
            Generation = _generation,
            VersionId = _versionId,
            Version = _version,
        };
}
