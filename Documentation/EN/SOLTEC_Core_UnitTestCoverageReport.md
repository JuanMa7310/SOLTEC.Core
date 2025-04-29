# SOLTEC.Core – Full Unit Test Coverage Report

This document confirms that **all public methods** of the logic-containing classes in the `SOLTEC.Core` project are covered by **unit tests** in both **NUnit** and **xUnit** environments.

---

## 🔐 Encryption

| Method                  | Unit Tested |
|-------------------------|-------------|
| Base64Encode            | ✅ Yes       |
| Base64Decode            | ✅ Yes       |
| CreateMD5               | ✅ Yes       |
| GenerateSHA1            | ✅ Yes       |
| GenerateSHA256          | ✅ Yes       |
| GenerateSHA384          | ✅ Yes       |
| GenerateSHA512          | ✅ Yes       |
| CreateTokenHMACSHA256   | ✅ Yes       |
| Token                   | ✅ Yes       |
| GenerateUniqueKey       | ✅ Yes       |

---

## 📁 FileManagment

| Method                  | Unit Tested |
|-------------------------|-------------|
| CreateFile              | ✅ Yes       |
| DeleteFile              | ✅ Yes       |
| EncodeFileToBase64      | ✅ Yes       |
| DecodeBase64ToStream    | ✅ Yes       |
| ExtractExtensionFileFromPath | ✅ Yes  |
| ExtractFileNameFromPath      | ✅ Yes  |
| CopyFile                     | ✅ Yes  |
| MoveFile                     | ✅ Yes  |

---

## 📦 ServiceResponse

| Method                  | Unit Tested |
|-------------------------|-------------|
| CreateSuccess           | ✅ Yes       |
| CreateError             | ✅ Yes       |
| CreateWarning           | ✅ Yes       |

---

## 📦 ServiceResponse<T>

| Method                  | Unit Tested |
|-------------------------|-------------|
| CreateSuccess           | ✅ Yes       |
| CreateError             | ✅ Yes       |
| CreateWarning           | ✅ Yes       |

---

## ❗ HttpCore

| Method                  | Unit Tested |
|-------------------------|-------------|
| GetAsync                | ✅ Yes       |
| GetAsyncList            | ✅ Yes       |
| PostAsync               | ✅ Yes       |
| PutAsync                | ✅ Yes       |
| DeleteAsync             | ✅ Yes       |

---

## ❗ HttpCoreException

| Constructor             | Unit Tested |
|-------------------------|-------------|
| HttpCoreException(...)  | ✅ Yes       |

---

## ❗ ResultException

| Constructor             | Unit Tested |
|-------------------------|-------------|
| ResultException(...)    | ✅ Yes       |

---

All unit tests have been implemented in both **NUnit** and **xUnit**, ensuring full and consistent validation of each public method in classes with internal logic.
