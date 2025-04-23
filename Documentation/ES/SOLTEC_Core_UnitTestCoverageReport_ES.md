# SOLTEC.Core – Informe completo de cobertura de pruebas unitarias

Este documento confirma que **todos los métodos públicos** de las clases con lógica en el proyecto `SOLTEC.Core` están cubiertos por **pruebas unitarias**, tanto en los entornos **NUnit** como **xUnit**.

---

## 🔐 Encryption

| Método                  | Probado |
|-------------------------|---------|
| Base64Encode            | ✅ Sí    |
| Base64Decode            | ✅ Sí    |
| CreateMD5               | ✅ Sí    |
| GenerateSHA1            | ✅ Sí    |
| GenerateSHA256          | ✅ Sí    |
| GenerateSHA384          | ✅ Sí    |
| GenerateSHA512          | ✅ Sí    |
| CreateTokenHMACSHA256   | ✅ Sí    |
| Token                   | ✅ Sí    |
| GenerateUniqueKey       | ✅ Sí    |

---

## 📁 FileManagment

| Método                  | Probado |
|-------------------------|---------|
| CreateFile              | ✅ Sí    |
| DeleteFile              | ✅ Sí    |
| EncodeFileToBase64      | ✅ Sí    |
| DecodeBase64ToStream    | ✅ Sí    |
| ExtractExtensionFileFromPath | ✅ Sí |
| ExtractFileNameFromPath      | ✅ Sí |
| CopyFile                     | ✅ Sí |
| MoveFile                     | ✅ Sí |

---

## 📦 ServiceResponse

| Método                  | Probado |
|-------------------------|---------|
| CreateSuccess           | ✅ Sí    |
| CreateError             | ✅ Sí    |
| CreateWarning           | ✅ Sí    |

---

## 📦 ServiceResponse<T>

| Método                  | Probado |
|-------------------------|---------|
| CreateSuccess           | ✅ Sí    |
| CreateError             | ✅ Sí    |
| CreateWarning           | ✅ Sí    |

---

## ❗ HttpCore

| Método                  | Probado |
|-------------------------|---------|
| GetAsync                | ✅ Sí    |
| GetAsyncList            | ✅ Sí    |
| PostAsync               | ✅ Sí    |
| PutAsync                | ✅ Sí    |
| DeleteAsync             | ✅ Sí    |

---

## ❗ HttpCoreException

| Constructor             | Probado |
|-------------------------|---------|
| HttpCoreException(...)  | ✅ Sí    |

---

## ❗ ResultException

| Constructor             | Probado |
|-------------------------|---------|
| ResultException(...)    | ✅ Sí    |

---

Todas las pruebas unitarias han sido implementadas en **NUnit** y **xUnit**, asegurando una validación completa y consistente de cada método público de las clases con lógica.
