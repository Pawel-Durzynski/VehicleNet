using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides engine architecture specifications.</summary>
public record EngineArchitecture
{
    /// <summary>Gets the number of cylinders.</summary>
    public ParameterValue CylinderCount { get; init; } = ParameterValue.Missing(MeasurementUnit.Count);

    /// <summary>Gets the cylinder arrangement.</summary>
    public string CylinderArrangement { get; init; } = string.Empty;

    /// <summary>Gets the number of valves.</summary>
    public ParameterValue ValveCount { get; init; } = ParameterValue.Missing(MeasurementUnit.Count);
}
