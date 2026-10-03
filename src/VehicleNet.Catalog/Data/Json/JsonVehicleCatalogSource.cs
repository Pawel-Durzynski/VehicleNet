using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using VehicleNet.Common.Models.Catalog;

namespace VehicleNet.Catalog.Data.Json;

/// <summary>
/// Represents a source of vehicle catalog data that loads the data from embedded JSON resources within the assembly.
/// </summary>
public sealed class JsonVehicleCatalogSource : IVehicleCatalogSource
{
    private readonly Assembly _assembly;
    private readonly CatalogSnapshotBuilder _builder;
    private readonly IReadOnlyList<VehicleBodyEngine>? _snapshot;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonVehicleCatalogSource"/> class.
    /// </summary>
    public JsonVehicleCatalogSource()
        : this(typeof(JsonVehicleCatalogSource).Assembly, new CatalogSnapshotBuilder())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonVehicleCatalogSource"/> class with the specified assembly and catalog snapshot builder.
    /// </summary>
    /// <param name="assembly">The assembly containing the embedded JSON resources.</param>
    /// <param name="builder">The catalog snapshot builder used to build the catalog snapshot.</param>
    internal JsonVehicleCatalogSource(Assembly assembly, CatalogSnapshotBuilder builder)
    {
        _assembly = assembly;
        _builder = builder;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonVehicleCatalogSource"/> class with the specified catalog snapshot.
    /// </summary>
    /// <param name="snapshot">The catalog snapshot to be used by the source.</param>
    internal JsonVehicleCatalogSource(IReadOnlyList<VehicleBodyEngine> snapshot)
        : this(typeof(JsonVehicleCatalogSource).Assembly, new CatalogSnapshotBuilder())
    {
        _snapshot = snapshot;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleBodyEngine>> LoadAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_snapshot is not null)
        {
            return _snapshot;
        }

        var document = new CatalogJsonDocument
        {
            Manufacturers = await LoadResourceAsync("1-manufacturers.json", context => context.IReadOnlyListManufacturerDto, cancellationToken),
            Models = await LoadResourceAsync("2-models.json", context => context.IReadOnlyListModelDto, cancellationToken),
            Generations = await LoadResourceAsync("3-generations.json", context => context.IReadOnlyListGenerationDto, cancellationToken),
            Versions = await LoadResourceAsync("4-versions.json", context => context.IReadOnlyListVersionDto, cancellationToken),
            Engines = await LoadResourceAsync("5-engines.json", context => context.IReadOnlyListEngineDto, cancellationToken),
            EngineVariants = await LoadResourceAsync("6-engine-variants.json", context => context.IReadOnlyListEngineVariantDto, cancellationToken),
            VehicleBodies = await LoadResourceAsync("7-vehicle-bodies.json", context => context.IReadOnlyListVehicleBodyDto, cancellationToken),
            VehicleBodyEngines = await LoadResourceAsync("8-vehicle-body-engines.json", context => context.IReadOnlyListVehicleBodyEngineDto, cancellationToken),
            VehicleBodyEngineVariants = await LoadResourceAsync("9-vehicle-body-engine-variants.json", context => context.IReadOnlyListVehicleBodyEngineVariantDto, cancellationToken)
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
