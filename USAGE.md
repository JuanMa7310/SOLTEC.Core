# 📘 Usage Guide for SOLTEC.Core

This document provides examples of how to use the most important classes in the SOLTEC.Core library.

---

## 🔁 ServiceResponse

```csharp
var response = ServiceResponse.CreateSuccess(200);
// With warning messages
var responseWithWarnings = ServiceResponse.CreateSuccess(200, new[] { "Minor issue" });
```

## 🔁 ServiceResponse<T>

```csharp
var dataResponse = ServiceResponse<string>.CreateSuccess("Operation completed", 200);
var errorResponse = ServiceResponse<string>.CreateError(400, "Invalid input");
```

## 🔐 Encryptions

```csharp
var enc = new Encryptions();
string hash = enc.GenerateSHA256("password");
string token = enc.CreateTokenHMACSHA256("message", "secret");
```

## 📂 FileManagment

```csharp
FileManagment.SaveText("output.txt", "Hello World");
string content = FileManagment.ReadText("output.txt");
```

## 🌐 HttpCore

```csharp
var client = new HttpCore();
var result = await client.GetAsync<string>("https://example.com/api/data");
```

## ⚠️ Exception Handling

```csharp
try {
    throw new ResultException("ERROR_KEY", new Exception("Something went wrong"));
} catch (ResultException ex) {
    Console.WriteLine(ex.Key);
}
```