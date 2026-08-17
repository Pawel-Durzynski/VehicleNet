# VehicleNet

VehicleNet is a modular, high-performance .NET library for automotive data. It provides robust tools for VIN (Vehicle Identification Number) validation, random VIN generation for testing purposes, and a comprehensive catalog of vehicle makes, models, and specifications.

## Features

VehicleNet is designed as a monorepo containing modular NuGet packages, allowing you to include only the dependencies you actually need:

*   **`VehicleNet.Vin`**: A lightweight library for validating, parsing, and generating algoritmically correct mock VINs.
*   **`VehicleNet.Catalog`**: A comprehensive vehicle database containing makes, models, generations, and basic specifications.

## Installation

Install the packages via the .NET CLI:

```bash
dotnet add package VehicleNet.Vin
dotnet add package VehicleNet.Catalog
```

## Architecture

This project uses a monorepo approach with shared MSBuild properties (`Directory.Build.props`) to ensure consistent versioning and metadata across all published packages.

## Contributing

Contributions are always welcome! Whether it's adding new models to the catalog, improving the VIN decoding algorithms, or fixing bugs:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please ensure that your code is covered by unit tests before submitting a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
