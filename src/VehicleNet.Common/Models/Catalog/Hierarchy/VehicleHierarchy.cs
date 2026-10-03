namespace VehicleNet.Common.Models.Catalog.Hierarchy;

/// <summary>Provides the display names that identify a vehicle's catalog hierarchy.</summary>
/// <param name="Manufacturer">The manufacturer name.</param>
/// <param name="Model">The model name.</param>
/// <param name="Generation">The generation name.</param>
/// <param name="Version">The version name.</param>
/// <param name="Engine">The engine name.</param>
/// <param name="EngineVersion">The engine variant name.</param>
public sealed record VehicleHierarchy(
    string Manufacturer,
    string Model,
    string Generation,
    string Version,
    string Engine,
    string EngineVersion);
