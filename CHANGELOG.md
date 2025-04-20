# 📝 CHANGELOG.md - SOLTEC.Core

All notable changes to this project will be documented in this file.

---

## [1.0.0] - 2025-04

### 🎉 Initial Release

- Introduced standardized response classes: `ServiceResponse`, `ServiceResponse<T>`
- Added encryption and hashing utilities: `Encryptions`
- Included HTTP abstraction layer: `HttpCore`
- Added custom exception types: `ResultException`, `HttpCoreException`
- File management class: `FileManagment`
- Complete unit tests with NUnit and xUnit
- PreBuild validator with:
  - XML documentation validation
  - TODO/FIXME detection
  - Test coverage detection
- GitHub Actions workflows and CLI scripts (`.sh`, `.bat`)