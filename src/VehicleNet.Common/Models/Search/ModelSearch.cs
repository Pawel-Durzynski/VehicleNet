namespace VehicleNet.Common.Models.Search;

public sealed record ModelSearch
{
    public int? ManufacturerId { get; init; }

    public string? Name { get; init; }
}
