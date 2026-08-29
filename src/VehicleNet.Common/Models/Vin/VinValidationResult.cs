namespace VehicleNet.Common.Models.Vin;

public sealed record VinValidationResult(bool IsValid, string? Error)
{
    public static VinValidationResult Success() => new(true, null);

    public static VinValidationResult Failure(string error) => new(false, error);
}
