# 📘 README: Scripts & Workflows

Este documento describe cómo usar los scripts locales y los flujos de trabajo (`workflows`) de GitHub Actions para el proyecto **SOLTEC.Core**.

---

## 🧪 PreBuild Validator

Valida que el proyecto cumpla con:

- Configuración `.csproj` (`LangVersion`, `Nullable`)
- Comentarios XML en clases públicas
- Ausencia de comentarios `TODO` o `FIXME`
- Presencia de clases de pruebas unitarias para cada clase lógica
- Que cada clase de prueba contenga métodos `[Fact]` o `[Test]`

---

## 🖥️ Scripts locales

| Script                  | Plataforma     | Ubicación recomendada    | Cómo usar                                  |
|-------------------------|----------------|---------------------------|---------------------------------------------|
| `run-validator.bat`     | Windows        | raíz del proyecto         | Doble clic o desde terminal `cmd`          |
| `run-validator.sh`      | Linux / Rider  | raíz del proyecto         | `chmod +x run-validator.sh && ./run-validator.sh` |

---

## ⚙️ GitHub Workflows

Colocar todos en `.github/workflows/`

| Archivo YAML                                  | ¿Cuándo se ejecuta?                              | ¿Qué hace?                                      |
|----------------------------------------------|--------------------------------------------------|-------------------------------------------------|
| `prebuild-validator.yml`                     | Push o PR a ramas `Master` o `DEV`              | Ejecuta el PreBuild Validator completo          |
| `prebuild-validator-pr-only.yml`             | Solo PR hacia `Master` o `DEV`                  | Igual que arriba pero no en push directo        |
| `prebuild-validator-path-filtered.yml`       | Solo PR y si hay cambios en `SOLTEC.Core` o `Tools/` | Optimiza para cambios relevantes           |
| `dotnet-format.yml`                          | En cada PR a `Master` o `DEV`                   | Valida el formato del código (`dotnet format`)  |
| `dotnet-analyzer.yml`                        | En cada PR a `Master` o `DEV`                   | Analiza errores con `dotnet build -warnaserror` |

---

## 📦 Requisitos

- .NET SDK 8.0 o superior
- GitHub Actions habilitado en el repositorio

---

## 🧭 Recomendación

Usar los scripts `.bat` o `.sh` durante desarrollo local. GitHub Actions validará automáticamente cada PR para garantizar calidad de código antes de hacer `merge`.