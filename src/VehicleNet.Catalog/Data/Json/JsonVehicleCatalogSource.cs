using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Catalog.Data.Json;

public sealed class JsonVehicleCatalogSource : IVehicleCatalogSource
{
    private readonly Assembly _assembly;
    private readonly CatalogSnapshotBuilder _builder;

    public JsonVehicleCatalogSource()
        : this(typeof(JsonVehicleCatalogSource).Assembly, new CatalogSnapshotBuilder())
    {
    }

    internal JsonVehicleCatalogSource(Assembly assembly, CatalogSnapshotBuilder builder)
    {
        _assembly = assembly;
        _builder = builder;
    }

    public async Task<IReadOnlyList<VehicleSpec>> LoadAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var document = new CatalogJsonDocument
        {
            Manufacturers = await LoadResourceAsync("manufacturers.json", context => context.IReadOnlyListManufacturerDto, cancellationToken),
            Models = await LoadResourceAsync("models.json", context => context.IReadOnlyListModelDto, cancellationToken),
            Generations = await LoadResourceAsync("generations.json", context => context.IReadOnlyListGenerationDto, cancellationToken),
            Versions = await LoadResourceAsync("versions.json", context => context.IReadOnlyListVersionDto, cancellationToken),
            Engines = await LoadResourceAsync("engines.json", context => context.IReadOnlyListEngineDto, cancellationToken),
            VehicleBodies = await LoadResourceAsync("vehicle-bodies.json", context => context.IReadOnlyListVehicleBodyDto, cancellationToken),
            VehicleSpecs = await LoadResourceAsync("vehicle-specs.json", context => context.IReadOnlyListVehicleSpecDto, cancellationToken)
        };

        return _builder.Build(document);
    }

    private async Task<T> LoadResourceAsync<T>(
        string fileName,
        Func<CatalogJsonSerializerContext, JsonTypeInfo<T>> jsonTypeInfoFactory,
        CancellationToken cancellationToken)
    {
        var resourceName = FindResourceName(_assembly, fileName);

        await using var stream = _assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Catalog resource '{resourceName}' was not found.");

        var result = await JsonSerializer.DeserializeAsync(
            stream,
            jsonTypeInfoFactory(CatalogJsonSerializerContext.Default),
            cancellationToken);

        return result is null
            ? throw new InvalidOperationException($"Catalog JSON resource '{fileName}' was empty or invalid.")
            : result;
    }

    private static string FindResourceName(Assembly assembly, string fileName)
    {
        var resourceName = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(name => name.EndsWith($"Data.{fileName}", StringComparison.Ordinal));

        return resourceName
            ?? throw new InvalidOperationException($"Embedded catalog JSON resource 'Data/{fileName}' was not found.");
    }
}
