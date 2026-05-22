
# AegisCore

Uma estrutura abrangente de verificação de integridade do sistema e segurança para aplicativos Windows.

## Arquitetura

O AegisCore é construído com uma arquitetura modular composta por vários componentes especializados:

- **AegisCore.Core** - Modelos e interfaces principais
- **AegisCore.Registry** - Implementação segura de provedor de registro
- **AegisCore.Scanning** - Mecanismo de verificação com regras conectáveis
- **AegisCore.Validation** - Validadores de arquivos e serviços
- **AegisCore.Risk** - Cálculo de confiança e risco
- **AegisCore.Rollback** - Gerenciamento de transações e quarentena
- **AegisCore.Storage** - Repositórios SQLite e persistência de dados
- **AegisCore.Logging** - Serilog com sinks configuráveis
- **AegisCore.Services** - Windows Service e Named Pipes
- **AegisCore.UI** - WPF com padrão MVVM
- **AegisCore.Installer** - WiX Toolset para empacotamento MSI

## Começando

### Pré-requisitos

- .NET 8 SDK
- Windows 10/11
- Visual Studio 2022 ou VS Code com extensão C#

### Compilar

```bash
dotnet build
```

### Executar Aplicação de Interface

```bash
dotnet run --project src/AegisCore.UI
```

### Executar Testes

```bash
dotnet test tests/AegisCore.Tests
```

## Estrutura do Projeto

```
AegisCore/
├── src/
│   ├── AegisCore.Core/          # Modelos, interfaces
│   ├── AegisCore.Registry/      # IRegistryProvider, implementação segura
│   ├── AegisCore.Scanning/      # Mecanismo de verificação, regras conectáveis
│   ├── AegisCore.Validation/    # Validadores de arquivos, validadores de serviços
│   ├── AegisCore.Risk/          # Cálculo de confiança/risco
│   ├── AegisCore.Rollback/      # Transações, quarentena
│   ├── AegisCore.Storage/       # SQLite, repositórios
│   ├── AegisCore.Logging/       # Serilog, sinks configuráveis
│   ├── AegisCore.Services/      # Windows Service, Named Pipes
│   ├── AegisCore.UI/            # WPF, MVVM, interface completa
│   └── AegisCore.Installer/     # WiX Toolset, MSI
├── tests/
│   └── AegisCore.Tests/         # xUnit, FluentAssertions
└── scripts/
    └── publish-github.ps1       # Script de versionamento e push
```

## Contribuindo

Consulte [CONTRIBUTING.md](CONTRIBUTING.md) para diretrizes.

## Licença

Consulte [LICENSE](LICENSE) para detalhes.

## Segurança

Consulte [SECURITY.md](SECURITY.md) para política de segurança.
