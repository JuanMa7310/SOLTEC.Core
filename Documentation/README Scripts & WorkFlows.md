# 📘 README: Scripts & Workflows

This document explains how to use the local scripts and GitHub Actions workflows for the **SOLTEC.Core** project.

---

## 🧪 PreBuild Validator

Validates that the project complies with:

- `.csproj` settings (`LangVersion`, `Nullable`)
- XML documentation on public classes
- Absence of `TODO` or `FIXME` comments
- Existence of test classes for each logical class
- That each test class contains `[Fact]` or `[Test]` methods

---

## 🖥️ Local Scripts

| Script               | Platform       | Recommended location | How to run                                     |
|----------------------|----------------|------------------------|------------------------------------------------|
| `run-validator.bat`  | Windows        | Project root           | Double-click or run via `cmd` or PowerShell    |
| `run-validator.sh`   | Linux / Rider  | Project root           | `chmod +x run-validator.sh && ./run-validator.sh` |

---

## ⚙️ GitHub Workflows

Place all of them in `.github/workflows/`

| YAML File                                 | When does it run?                             | What does it validate?                             |
|-------------------------------------------|------------------------------------------------|-----------------------------------------------------|
| `prebuild-validator.yml`                  | On push or PR to `Master` or `DEV` branches   | Full execution of the PreBuild Validator            |
| `prebuild-validator-pr-only.yml`          | Only on PR to `Master` or `DEV`               | Same validation, but skips direct push              |
| `prebuild-validator-path-filtered.yml`    | Only PR and only when files in `SOLTEC.Core` or `Tools/` are modified | Optimized execution           |
| `dotnet-format.yml`                       | On every PR to `Master` or `DEV`              | Validates code formatting using `dotnet format`     |
| `dotnet-analyzer.yml`                     | On every PR to `Master` or `DEV`              | Runs static analysis via `dotnet build -warnaserror` |

---

## 📦 Requirements

- .NET SDK 8.0 or newer
- GitHub Actions enabled in the repository

---

## 🧭 Recommendation

Use `.bat` or `.sh` scripts during local development. GitHub Actions will automatically validate each PR to ensure code quality before merging.