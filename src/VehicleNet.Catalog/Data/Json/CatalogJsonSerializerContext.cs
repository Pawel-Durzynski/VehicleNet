using System.Text.Json.Serialization;

namespace VehicleNet.Catalog.Data.Json;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    UseStringEnumConverter = true,
    WriteIndented = true)]
[JsonSerializable(typeof(CatalogJsonDocument))]
[JsonSerializable(typeof(IReadOnlyList<ManufacturerDto>))]
[JsonSerializable(typeof(IReadOnlyList<ModelDto>))]
[JsonSerializable(typeof(IReadOnlyList<GenerationDto>))]
[JsonSerializable(typeof(IReadOnlyList<VersionDto>))]
[JsonSerializable(typeof(IReadOnlyList<EngineDto>))]
[JsonSerializable(typeof(IReadOnlyList<EngineVariantDto>))]
[JsonSerializable(typeof(IReadOnlyList<VehicleBodyDto>))]
[JsonSerializable(typeof(IReadOnlyList<VehicleBodyEngineDto>))]
[JsonSerializable(typeof(IReadOnlyList<VehicleBodyEngineVariantDto>))]
internal sealed partial class CatalogJsonSerializerContext : JsonSerializerContext
{
}
