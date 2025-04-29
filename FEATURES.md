# ✨ FEATURES.md - SOLTEC.Core v1.0.0

Listado de funcionalidades disponibles en la versión `1.0.0` de la librería `SOLTEC.Core`.

---

## 📚 Funcionalidades principales

| Categoría           | Funcionalidad                                                    | Clase responsable             |
|---------------------|------------------------------------------------------------------|-------------------------------|
| 🔁 Respuesta        | Estructura de respuesta genérica con éxito/error                | `ServiceResponse`             |
| 🔁 Respuesta + datos| Estructura de respuesta genérica con datos y metainformación    | `ServiceResponse<T>`          |
| 🔐 Cifrado          | Hashing con SHA1, SHA256, SHA384, SHA512, MD5                   | `Encryptions`                 |
| 🔑 Tokens           | Generación de HMAC SHA256 y tokens únicos                       | `Encryptions`                 |
| 🔠 Codificación     | Base64 Encode/Decode                                            | `Encryptions`                 |
| 📡 HTTP             | Cliente HTTP (GET, POST, PUT, DELETE) con validación de errores | `HttpCore`                    |
| 🗂️ Archivos         | Escritura, lectura, codificación de archivos                    | `FileManagment`               |
| 🧪 Prebuild         | Validación previa a compilación (estructura, pruebas, docs)     | `SOLTEC.Core.PreBuildValidator` |
| ⚠️ Excepciones      | Excepciones enriquecidas con código, razón y estado HTTP         | `ResultException`, `HttpCoreException` |
| 🧪 Pruebas Unitarias| xUnit y NUnit para todas las clases lógicas                     | `SOLTEC.Core.Tests.*`         |
| 🧰 Integración CI   | Workflows para GitHub Actions y scripts para Rider/VS           | `.github/workflows/`, `*.sh`, `*.bat` |

---

## 🔖 Información de versión

- Versión: `1.0.0`
- Estado: Estable
- Última actualización: Abril 2025

---

## 🔗 Enlaces útiles

- [README.md (Inglés)](README.md)
- [README_ES.md (Castellano)](README_ES.md)