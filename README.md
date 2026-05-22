
# AegisCore

A comprehensive system integrity scanning and security framework for Windows applications.

## Architecture

AegisCore is built with a modular architecture consisting of multiple specialized components:

- **AegisCore.Core** - Core models and interfaces
- **AegisCore.Registry** - Secure registry provider implementation
- **AegisCore.Scanning** - Scanner engine with pluggable rules
- **AegisCore.Validation** - File and service validators
- **AegisCore.Risk** - Trust and risk calculation
- **AegisCore.Rollback** - Transaction management and quarantine
- **AegisCore.Storage** - SQLite repositories and data persistence
- **AegisCore.Logging** - Serilog with configurable sinks
- **AegisCore.Services** - Windows Service and Named Pipes
- **AegisCore.UI** - WinUI 3 with MVVM pattern
- **AegisCore.Installer** - WiX Toolset for MSI packaging

## Getting Started

### Prerequisites

- .NET 8 SDK
- Windows 10/11
- Visual Studio 2022 or VS Code with C# extension

### Build

```bash
dotnet build
```

### Run UI Application

```bash
dotnet run --project src/AegisCore.UI
```

### Run Tests

```bash
dotnet test tests/AegisCore.Tests
```

## Project Structure

```
AegisCore/
├── src/
│   ├── AegisCore.Core/          # Models, interfaces
│   ├── AegisCore.Registry/      # IRegistryProvider, secure implementation
│   ├── AegisCore.Scanning/      # Scanner engine, pluggable rules
│   ├── AegisCore.Validation/    # File validators, service validators
│   ├── AegisCore.Risk/          # Trust/risk calculation
│   ├── AegisCore.Rollback/      # Transactions, quarantine
│   ├── AegisCore.Storage/       # SQLite, repositories
│   ├── AegisCore.Logging/       # Serilog, configurable sinks
│   ├── AegisCore.Services/      # Windows Service, Named Pipes
│   ├── AegisCore.UI/            # WinUI 3, MVVM, complete interface
│   └── AegisCore.Installer/     # WiX Toolset, MSI
├── tests/
│   └── AegisCore.Tests/         # xUnit, FluentAssertions
└── scripts/
    └── publish-github.ps1       # Versioning and push script
```

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

See [LICENSE](LICENSE) for details.

## Security

See [SECURITY.md](SECURITY.md) for security policy.
