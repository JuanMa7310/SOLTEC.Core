# SOLTEC.Core

## SOLTEC.Core.PreBuildValidator

Console tool for pre-build validation of SOLTEC.Core projects. Compatible with Visual Studio 2022, JetBrains Rider, and GitHub Actions.

---

### ✅ What does it validate?

1. All `.csproj` files must have:
   - `LangVersion = 12.0`
   - `Nullable = enable`

2. All **public classes** must have XML documentation (`///`)
3. No `TODO` or `FIXME` comments pending
4. At least one `[Fact]` or `[Test]` method must exist in test projects
5. (Optional) Project compiles successfully (this step is now skipped for performance)

---

### ⚙️ Manual Usage

```bash
dotnet build Tools/SOLTEC.Core.PreBuildValidator
dotnet Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll
```

---

### 🧪 Integration with Visual Studio

Add this to your main `.csproj` (e.g., `SOLTEC.Core.csproj`):

```xml
<Target Name="RunPreBuildValidator" BeforeTargets="BeforeBuild" Condition=" '$(GITHUB_ACTIONS)' != 'true' ">
  <Exec Command="dotnet $(SolutionDir)Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll" />
</Target>
```

---

### 🧩 Integration with JetBrains Rider

1. Go to **File > Settings > Build, Execution, Deployment > Build Tools > Before Build**
2. Add a new external tool:
   - **Name**: `Run PreBuild Validator`
   - **Program**: `dotnet`
   - **Arguments**: `Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll`
   - **Working Directory**: `$ProjectFileDir$`

---

### 🚀 Integration with GitHub Actions

Add this file as `.github/workflows/validator.yml`:

```yaml
name: PreBuild Validator

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  validate-project:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout
      uses: actions/checkout@v3

    - name: Setup .NET 8
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'

    - name: Build Validator
      run: dotnet build Tools/SOLTEC.Core.PreBuildValidator

    - name: Run Validator
      run: dotnet Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll
```

---

Use this to prevent bad configurations from reaching your main branch!