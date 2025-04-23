# SOLTEC.Core – Full Integration Test Method Report

Este informe confirma que **todos los métodos** públicos de las clases con lógica en `SOLTEC.Core` están cubiertos por pruebas unitarias de integración en los entornos **NUnit** y **xUnit**.

---

## 🔐 Encryption

| Método                   | Cubierto |
|--------------------------|----------|
| Base64Encode             | ✅ Sí     |
| Base64Decode             | ✅ Sí     |
| CreateMD5                | ✅ Sí     |
| GenerateSHA1             | ✅ Sí     |
| GenerateSHA256           | ✅ Sí     |
| GenerateSHA384           | ✅ Sí     |
| GenerateSHA512           | ✅ Sí     |
| CreateTokenHMACSHA256    | ✅ Sí     |
| Token                    | ✅ Sí     |
| GenerateUniqueKey        | ✅ Sí     |

---

## 📁 FileManagment

| Método                   | Cubierto |
|--------------------------|----------|
| CreateFile               | ✅ Sí     |
| DeleteFile               | ✅ Sí     |
| EncodeFileToBase64       | ✅ Sí     |
| DecodeBase64ToStream     | ✅ Sí     |

---

## 📦 ServiceResponse

| Método                   | Cubierto |
|--------------------------|----------|
| CreateSuccess            | ✅ Sí     |
| CreateError              | ✅ Sí     |
| CreateWarning            | ✅ Sí     |

---

## 📦 ServiceResponse<T>

| Método                   | Cubierto |
|--------------------------|----------|
| CreateSuccess            | ✅ Sí     |
| CreateError              | ✅ Sí     |
| CreateWarning            | ✅ Sí     |

---

## ❗ HttpCoreException

| Elemento                 | Cubierto |
|--------------------------|----------|
| Constructor              | ✅ Sí     |

---

## ❗ ResultException

| Elemento                 | Cubierto |
|--------------------------|----------|
| Constructor              | ✅ Sí     |

---

Todas las pruebas de integración han sido implementadas tanto en **NUnit** como en **xUnit**, asegurando la validación completa y consistente del comportamiento de cada clase pública con lógica.
