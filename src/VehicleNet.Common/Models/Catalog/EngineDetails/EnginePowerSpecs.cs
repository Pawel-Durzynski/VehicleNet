using VehicleNet.Common.Models.Units;

namespace VehicleNet.Common.Models.Catalog.EngineDetails;

/// <summary>Provides engine power specifications.</summary>
public record EnginePowerSpecs
{
    /// <summary>Gets the engine power in horsepower.</summary>
    public ParameterValue Horsepower { get; init; } = ParameterValue.Missing(MeasurementUnit.Horsepower);

    /// <summary>Gets the engine speed at which the stated power is produced.</summary>
    public ParameterValue AtRpm { get; init; } = ParameterValue.Missing(MeasurementUnit.Rpm);
}
