# ✨ FEATURES_ES.md - SOLTEC.Core v1.0.0

Listado de funcionalidades disponibles en la versión `1.0.0` de la librería `SOLTEC.Core`.

---

## 📚 Funcionalidades principales

| Categoría           | Funcionalidad                                                    | Clase responsable             |
|---------------------|------------------------------------------------------------------|-------------------------------|
| 🔁 Respuesta        | Estructura de respuesta genérica con éxito/error                | `ServiceResponse`             |
| 🔁 Respuesta + datos| Estructura de respuesta genérica con datos y metainformación    | `ServiceResponse<T>`          |
| 🔐 Cifrado          | Hashing con SHA1, SHA256, SHA384, SHA512, MD5                   | `Encryptions`                 |
| 🔑 Tokens           | Generación de HMAC SHA256 y tokens únicos                       | `Encryptions`                 |
| 🔠 Codificación     | Codificación y decodificación en Base64                         | `Encryptions`                 |
| 📡 HTTP             | Cliente HTTP genérico con validación de estado y cuerpo         | `HttpCore`                    |
| 🗂️ Archivos         | Gestión de archivos: guardar, leer, codificar, eliminar         | `FileManagment`               |
| 🧪 Prebuild         | Validación previa a compilación (docs, pruebas, estilo)         | `SOLTEC.Core.PreBuildValidator` |
| ⚠️ Excepciones      | Excepciones personalizadas para HTTP y errores de negocio        | `ResultException`, `HttpCoreException` |
| 🧪 Pruebas Unitarias| Tests unitarios con NUnit y xUnit para clases lógicas           | `SOLTEC.Core.Tests.*`         |
| 🧰 CI Integración   | Scripts de validación y workflows de GitHub Actions             | `.github/workflows/`, `*.sh`, `*.bat` |

---

## 🔖 Información de versión

- Versión: `1.0.0`
- Estado: Estable
- Última actualización: Abril 2025

---

## 🔗 Enlaces útiles

- [README_ES.md](README_ES.md)
- [README.md (English)](README.md)