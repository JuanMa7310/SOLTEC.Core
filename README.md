# SOLTEC.Core

SOLTEC.Core is a reusable library for .NET 8 that provides standardized response models, encryption utilities, HTTP communication management, custom services and exceptions, and tools for enterprise applications.

---

## 📌 License

[![GPLv3 License](https://www.gnu.org/graphics/gplv3-127x51.png)](https://www.gnu.org/licenses/gpl-3.0.html)

Licensed under GNU GPL v3.

---

## 🧩 Modules

- 🔐 Encryption utilities (MD5, SHA, HMAC, Base64)
- 📄 Standardized service responses with error/warning handling
- 🌐 HTTP request abstraction with automatic validation
- 📦 File management helpers
- 📊 Pre-build validation CLI
- 📚 Wiki documentation generator

---

## 🧪 Unit Tested

All components are fully tested using **xUnit** and **NUnit**, with coverage for success and failure scenarios.

---

## 🛠️ Pre-Build Validation

The included tool `SOLTEC.Core.PreBuildValidator` performs the following checks before any build or commit:

- Validates `LangVersion` = 12.0 and `<Nullable>enable</Nullable>`
- Ensures all public classes, methods, and properties have XML documentation
- Flags unresolved `TODO` and `FIXME` comments
- Verifies test coverage by matching unit test classes with production logic
- Validates that each test class has at least one test method
- ✅ **NEW:** Validates that each unit test method explicitly describes what it sends, expects, and what is asserted.

Example of compliant unit test:

```csharp
/// <summary>
/// Ensures that CreateSuccess returns a success result.
/// </summary>
/// <remarks>
/// Sends: A valid response code.
/// Expects: A ServiceResponse with Success = true.
/// Asserts: That the response is successful and contains the code.
/// </remarks>
[Test]
public void CreateSuccess_ShouldReturnSuccess()
{
    var result = ServiceResponse.CreateSuccess(200);
    Assert.IsTrue(result.Success);
    Assert.AreEqual(200, result.ResponseCode);
}
```

### 🛠 Usage

To validate your solution before building, run:

```bash
dotnet run --project Tools/SOLTEC.Core.PreBuildValidator
```

---

## 📘 Usage Examples

See [Usage Guide](USAGE.md) for code samples and common patterns.

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

### 🧾 Wiki Documentation Generator

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

---

## 📥 Contributing & Guidelines

- [Integration Guide](INTEGRATION.md)
- [Contributing Guide](CONTRIBUTING.md)
- [Security Policy](SECURITY.md)
- [Code of Conduct](CODE_OF_CONDUCT.md)
- [Coding Rules](CODING_RULES.md)
