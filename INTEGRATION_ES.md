# SOLTEC.Core

## SOLTEC.Core.PreBuildValidator

Herramienta de consola para la validación previa a la compilación de los proyectos SOLTEC.Core. Compatible con Visual Studio 2022, JetBrains Rider y GitHub Actions.

---

### ✅ ¿Qué valida?

1. Todos los archivos `.csproj` deben tener:
   - `LangVersion = 12.0`
   - `Nullable = enable`

2. Todas las **clases públicas** deben tener documentación XML (`///`)
3. No deben existir comentarios `TODO` o `FIXME` pendientes
4. Debe existir al menos un método `[Fact]` o `[Test]` en los proyectos de prueba
5. (Opcional) El proyecto compila correctamente (este paso está deshabilitado por rendimiento)

---

### ⚙️ Uso manual

```bash
dotnet build Tools/SOLTEC.Core.PreBuildValidator
dotnet Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll
```

---

### 🧪 Integración con Visual Studio

Agrega esto a tu archivo principal `.csproj` (por ejemplo: `SOLTEC.Core.csproj`):

```xml
<Target Name="RunPreBuildValidator" BeforeTargets="BeforeBuild" Condition=" '$(GITHUB_ACTIONS)' != 'true' ">
  <Exec Command="dotnet $(SolutionDir)Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll" />
</Target>
```

---

### 🧩 Integración con JetBrains Rider

1. Ve a **File > Settings > Build, Execution, Deployment > Build Tools > Before Build**
2. Agrega una nueva herramienta externa:
   - **Nombre**: `Run PreBuild Validator`
   - **Programa**: `dotnet`
   - **Argumentos**: `Tools/SOLTEC.Core.PreBuildValidator/bin/Debug/net8.0/SOLTEC.Core.PreBuildValidator.dll`
   - **Directorio de trabajo**: `$ProjectFileDir$`

---

### 🚀 Integración con GitHub Actions

Agrega este archivo como `.github/workflows/validator.yml`:

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

¡Utiliza esto para evitar que configuraciones incorrectas lleguen a tu rama principal!