# SOLTEC.Core

Una biblioteca sólida y extensible que ofrece modelos de respuesta estandarizados, utilidades de cifrado, gestión de comunicación HTTP y excepciones personalizadas para aplicaciones .NET 8.

---

## 📌 Licencia

[![Licencia GPLv3](https://www.gnu.org/graphics/gplv3-127x51.png)](https://www.gnu.org/licenses/gpl-3.0.html)

Licenciado bajo GNU GPL v3.

---

## 🧩 Módulos

- `ServiceResponse<T>`: Contenedor genérico de respuestas
- `Encryptions`: Utilidades de seguridad y hash
- `HttpCore`: Cliente HTTP tipado
- `ResultException`, `HttpCoreException`: Excepciones personalizadas

---

## 📘 Ejemplos de uso

Consulta la [Guía de Uso](USAGE_ES.md) para ver ejemplos de código y patrones comunes.

---

## 🧪 Pruebas Unitarias

Todos los componentes están completamente testeados con **xUnit** y **NUnit**, incluyendo escenarios de éxito y error.

---

## 🛠️ Validación Pre-Build

Este proyecto incluye un validador pre-build personalizado:
- Verifica que todas las clases públicas tengan documentación XML
- Revisa TODOs / FIXMEs sin resolver
- Confirma que todos los proyectos de test estén estructurados correctamente

---

## ⚙️ Instalación

Para usar **SOLTEC.Core** en tu proyecto .NET 8:

### 1. Añadir referencia al proyecto local

Si tienes la librería local:

```bash
dotnet add reference ../SOLTEC.Core/SOLTEC.Core.csproj
```

### 2. O instalar desde NuGet (cuando esté disponible)

```bash
dotnet add package SOLTEC.Core
```

### 3. Usar en código

```csharp
using SOLTEC.Core;

// Ejemplo de uso
var respuesta = ServiceResponse.CreateSuccess(200);
```

---

## 📘 Wiki multilingüe

Documentación completa disponible en **inglés y español**.

📖 [Navegación en Inglés](README_WIKI.md)  
📖 [Navegación en Español](README_WIKI_ES.md)

---

## 🧾 Generador de Documentación para la Wiki

La solución incluye una herramienta para generar documentación Markdown de todas las clases públicas y enumeraciones en inglés y español.

📂 Ubicación de la herramienta: `Tools/SOLTEC.Core.WikiDocGen`

Para ejecutarla:

```bash
dotnet run --project ./Tools/SOLTEC.Core.WikiDocGen
```

También disponible mediante:

- `run-wiki-generator.bat` (Windows)
- `run-wiki-generator.sh` (Linux/Rider)

La salida se encuentra en `DOCS/en/` y `DOCS/es/`.


## 📥 Contribuciones y Normas

- [Guía de Contribución](CONTRIBUTING_ES.md)
- [Política de Seguridad](SECURITY_ES.md)
- [Código de Conducta](CODE_OF_CONDUCT_ES.md)
