using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

public record EngineArchitecture
{
    public ParameterValue CylinderCount { get; init; } = ParameterValue.Missing(MeasurementUnit.Count);

    public string CylinderArrangement { get; init; } = string.Empty;

    public ParameterValue ValveCount { get; init; } = ParameterValue.Missing(MeasurementUnit.Count);
}
