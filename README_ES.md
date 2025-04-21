# SOLTEC.Core

SOLTEC.Core es una biblioteca reutilizable para .NET 8 que proporciona modelos de respuesta estandarizados, utilidades de cifrado, gestión de comunicación HTTP, servicios y excepciones personalizadas y herramientas para aplicaciones empresariales.

---

## 📌 Licencia

[![Licencia GPLv3](https://www.gnu.org/graphics/gplv3-127x51.png)](https://www.gnu.org/licenses/gpl-3.0.html)

Licenciado bajo GNU GPL v3.

---

## 🧩 Módulos

- 🔐 Utilidades de encriptación (MD5, SHA, HMAC, Base64)
- 📄 Respuestas de servicio estandarizadas con manejo de errores y advertencias
- 🌐 Abstracción para peticiones HTTP con validación automática
- 📦 Ayudantes para gestión de archivos
- 📊 Validador Pre-Build
- 📚 Generador de documentación para la Wiki de GitHub

---

## 🧪 Pruebas Unitarias

Todos los componentes están completamente testeados con **xUnit** y **NUnit**, incluyendo escenarios de éxito y error.

---

## 🛠️ Validación Pre-Build

La herramienta `SOLTEC.Core.PreBuildValidator` realiza las siguientes comprobaciones antes de cada build o commit:

- Verifica `LangVersion = 12.0` y `<Nullable>enable</Nullable>`
- Asegura que todas las clases, métodos y propiedades públicas tengan documentación XML
- Detecta comentarios sin resolver `TODO` y `FIXME`
- Valida que existan clases de prueba por cada clase lógica pública
- Verifica que cada clase de prueba tenga al menos un método de prueba
- ✅ **NUEVO:** Verifica que cada método de prueba documente explícitamente qué envía, qué espera y qué se valida.

Ejemplo de prueba válida:

```csharp
/// <summary>
/// Verifica que CreateSuccess devuelva un resultado correcto.
/// </summary>
/// <remarks>
/// Envia: Un código de respuesta válido.
/// Espera: Un ServiceResponse con Success = true.
/// Valida: Que el resultado sea correcto y tenga el código indicado.
/// </remarks>
[Test]
public void CreateSuccess_ShouldReturnSuccess()
{
    var result = ServiceResponse.CreateSuccess(200);
    Assert.IsTrue(result.Success);
    Assert.AreEqual(200, result.ResponseCode);
}
```

---

### 🛠 Uso

Para validar tu solución antes de compilar, ejecuta:

```bash
dotnet run --project Tools/SOLTEC.Core.PreBuildValidator
```

---

## 📘 Ejemplos de uso

Consulta la [Guía de Uso](USAGE_ES.md) para ver ejemplos de código y patrones comunes.

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

### 🧾 Generador de Documentación para la Wiki

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

---

## 📥 Contribuciones y Normas

- [Guía de Integración](INTEGRATION_ES.md)
- [Guía de Contribución](CONTRIBUTING_ES.md)
- [Política de Seguridad](SECURITY_ES.md)
- [Código de Conducta](CODE_OF_CONDUCT_ES.md)
