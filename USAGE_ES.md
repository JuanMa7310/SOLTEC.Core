# 📘 Guía de uso para SOLTEC.Core

Este documento proporciona ejemplos sobre cómo usar las clases más importantes de la biblioteca SOLTEC.Core.

---

## 🔁 ServiceResponse

```csharp
var respuesta = ServiceResponse.CreateSuccess(200);
// Con advertencias
var respuestaConAvisos = ServiceResponse.CreateSuccess(200, new[] { "Problema menor" });
```

## 🔁 ServiceResponse<T>

```csharp
var respuestaDatos = ServiceResponse<string>.CreateSuccess("Operación completada", 200);
var errorRespuesta = ServiceResponse<string>.CreateError(400, "Entrada inválida");
```

## 🔐 Encryptions

```csharp
var enc = new Encryptions();
string hash = enc.GenerateSHA256("contraseña");
string token = enc.CreateTokenHMACSHA256("mensaje", "secreto");
```

## 📂 FileManagment

```csharp
FileManagment.SaveText("salida.txt", "Hola Mundo");
string contenido = FileManagment.ReadText("salida.txt");
```

## 🌐 HttpCore

```csharp
var cliente = new HttpCore();
var resultado = await cliente.GetAsync<string>("https://example.com/api/data");
```

## ⚠️ Manejo de excepciones

```csharp
try {
    throw new ResultException("ERROR_CLAVE", new Exception("Algo salió mal"));
} catch (ResultException ex) {
    Console.WriteLine(ex.Key);
}
```