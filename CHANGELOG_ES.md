# 📝 CHANGELOG_ES.md - SOLTEC.Core

Todos los cambios relevantes se documentarán en este archivo.

---

## [1.0.0] - Abril 2025

### 🎉 Versión Inicial

- Incorporación de clases de respuesta estandarizadas: `ServiceResponse`, `ServiceResponse<T>`
- Añadidas utilidades de cifrado y hashing: `Encryptions`
- Capa de abstracción HTTP con validación de respuestas: `HttpCore`
- Excepciones personalizadas para errores de lógica y HTTP: `ResultException`, `HttpCoreException`
- Clase de gestión de archivos: `FileManagment`
- Cobertura completa de pruebas unitarias con NUnit y xUnit
- Validador de precompilación que revisa:
  - Comentarios XML
  - Comentarios `TODO` o `FIXME`
  - Cobertura de pruebas unitarias
- Workflows de GitHub Actions y scripts CLI (`.sh`, `.bat`)