namespace VehicleNet.Common.Models.Vin;

/// <summary>Represents the result of VIN validation.</summary>
/// <param name="IsValid">Indicates whether the VIN is valid.</param>
/// <param name="Error">The validation error, when validation fails.</param>
public sealed record VinValidationResult(bool IsValid, string? Error)
{
    /// <summary>Creates a successful validation result.</summary>
    public static VinValidationResult Success() => new(true, null);

    /// <summary>Creates a failed validation result.</summary>
    /// <param name="error">The validation error.</param>
    public static VinValidationResult Failure(string error) => new(false, error);
}
