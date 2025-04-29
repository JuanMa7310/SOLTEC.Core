# SOLTEC.Core – Full Integration Test Method Report

This report confirms that **all public methods** of the logic-containing classes in the `SOLTEC.Core` project are covered by **integration unit tests** in both **NUnit** and **xUnit** environments.

---

## 🔐 Encryption

| Method                  | Covered |
|-------------------------|---------|
| Base64Encode            | ✅ Yes  |
| Base64Decode            | ✅ Yes  |
| CreateMD5               | ✅ Yes  |
| GenerateSHA1            | ✅ Yes  |
| GenerateSHA256          | ✅ Yes  |
| GenerateSHA384          | ✅ Yes  |
| GenerateSHA512          | ✅ Yes  |
| CreateTokenHMACSHA256   | ✅ Yes  |
| Token                   | ✅ Yes  |
| GenerateUniqueKey       | ✅ Yes  |

---

## 📁 FileManagment

| Method                  | Covered |
|-------------------------|---------|
| CreateFile              | ✅ Yes  |
| DeleteFile              | ✅ Yes  |
| EncodeFileToBase64      | ✅ Yes  |
| DecodeBase64ToStream    | ✅ Yes  |

---

## 📦 ServiceResponse

| Method                  | Covered |
|-------------------------|---------|
| CreateSuccess           | ✅ Yes  |
| CreateError             | ✅ Yes  |
| CreateWarning           | ✅ Yes  |

---

## 📦 ServiceResponse<T>

| Method                  | Covered |
|-------------------------|---------|
| CreateSuccess           | ✅ Yes  |
| CreateError             | ✅ Yes  |
| CreateWarning           | ✅ Yes  |

---

## ❗ HttpCoreException

| Element                 | Covered |
|-------------------------|---------|
| Constructor             | ✅ Yes  |

---

## ❗ ResultException

| Element                 | Covered |
|-------------------------|---------|
| Constructor             | ✅ Yes  |

---

All integration tests have been implemented in both **NUnit** and **xUnit**, ensuring full and consistent validation of the behavior of each public logic-containing class.
