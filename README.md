# SOLTEC.Core

A robust and extensible library offering standardized response models, encryption utilities, HTTP communication handlers, and exception management for .NET 8 applications.

---

## 📌 License

[![GPLv3 License](https://www.gnu.org/graphics/gplv3-127x51.png)](https://www.gnu.org/licenses/gpl-3.0.html)

Licensed under GNU GPL v3.

---

## 🧩 Modules

- `ServiceResponse<T>`: Generic response wrapper
- `Encryptions`: Hash and security utilities
- `HttpCore`: Typed HTTP client
- `ResultException`, `HttpCoreException`: Custom exceptions

---

## 📘 Usage Examples

See [Usage Guide](USAGE.md) for code samples and common patterns.

---

## 🧪 Unit Tested

All components are fully tested using **xUnit** and **NUnit**, with coverage for success and failure scenarios.

---

## 🛠️ Pre-Build Validation

This project includes a custom pre-build validator:
- Ensures all public classes have XML documentation
- Checks for unresolved TODOs / FIXMEs
- Confirms all test projects are correctly structured

---

## ⚙️ Installation

To use **SOLTEC.Core** in your .NET 8 project:

### 1. Add project reference

If you have the library locally:

```bash
dotnet add reference ../SOLTEC.Core/SOLTEC.Core.csproj
```

### 2. Or add from NuGet (when available)

```bash
dotnet add package SOLTEC.Core
```

### 3. Use in code

```csharp
using SOLTEC.Core;

// Example usage
var response = ServiceResponse.CreateSuccess(200);
```

---

## 📘 Multilanguage Wiki

Full documentation is available in **English and Spanish**.

📖 [English Wiki Navigation](README_WIKI.md)  
📖 [Navegación en Español](README_WIKI_ES.md)

---

## 🧾 Wiki Documentation Generator

The solution includes a tool to generate Markdown documentation for all public classes and enums in both English and Spanish.

📂 Tool Location: `Tools/SOLTEC.Core.WikiDocGen`

To execute:

```bash
dotnet run --project ./Tools/SOLTEC.Core.WikiDocGen
```

Also available via:

- `run-wiki-generator.bat` (Windows)
- `run-wiki-generator.sh` (Linux/Rider)

The output will be located in `DOCS/en/` and `DOCS/es/`.


## 📥 Contributing & Guidelines

- [Contributing Guide](CONTRIBUTING.md)
- [Security Policy](SECURITY.md)
- [Code of Conduct](CODE_OF_CONDUCT.md)
