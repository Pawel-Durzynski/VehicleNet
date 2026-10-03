namespace VehicleNet.Common.Models.Search;

/// <summary>Builds vehicle body search criteria.</summary>
public sealed class VehicleBodySearchCriteriaBuilder
{
    private int? _vehicleBodyId;
    private string? _manufacturer;
    private string? _model;
    private int? _generationId;
    private string? _generation;
    private int? _versionId;
    private string? _version;

    /// <summary>Sets the vehicle body identifier filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithVehicleBodyId(int vehicleBodyId)
    {
        _vehicleBodyId = vehicleBodyId;
        return this;
    }

    /// <summary>Sets the manufacturer name filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithManufacturer(string manufacturer)
    {
        _manufacturer = manufacturer;
        return this;
    }

    /// <summary>Sets the model name filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithModel(string model)
    {
        _model = model;
        return this;
    }

    /// <summary>Sets the generation identifier filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithGenerationId(int generationId)
    {
        _generationId = generationId;
        return this;
    }

    /// <summary>Sets the generation name filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithGeneration(string generation)
    {
        _generation = generation;
        return this;
    }

    /// <summary>Sets the version identifier filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithVersionId(int versionId)
    {
        _versionId = versionId;
        return this;
    }

    /// <summary>Sets the version name filter.</summary>
    public VehicleBodySearchCriteriaBuilder WithVersion(string version)
    {
        _version = version;
        return this;
    }

    /// <summary>Builds the configured search criteria.</summary>
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
