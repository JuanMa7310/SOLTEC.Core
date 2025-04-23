
# Casos de Uso - Proyecto SOLTEC.Core - Pruebas Unitarias (Castellano)

Este documento recopila todos los casos de uso cubiertos por las pruebas unitarias implementadas en el proyecto SOLTEC.Core. Incluye pruebas para las siguientes clases:

- `HttpCore`
- `HttpCoreException`
- `HttpCoreErrorEnum`

---

## Clase: HttpCore

### Caso 1: GetAsync<T> retorna objeto deserializado
- **Qué hace:** Realiza una solicitud GET simulada y devuelve un objeto correctamente deserializado.
- **Qué se envía:** Una URL simulada con una respuesta JSON simulada.
- **Qué se espera:** Que el objeto resultante no sea nulo y contenga los datos esperados.

### Caso 2: PostAsync<T, TResult> retorna respuesta deserializada
- **Qué hace:** Simula un POST con un objeto en el cuerpo y devuelve una respuesta esperada.
- **Qué se envía:** Un objeto serializado.
- **Qué se espera:** Que la respuesta contenga los datos esperados.

### Caso 3: ValidateResult lanza HttpCoreException si ProblemDetailsDto indica error
- **Qué se envía:** Un JSON válido con status 400.
- **Qué se espera:** Se lanza `HttpCoreException`.

### Caso 4: ValidateStatusResponse lanza excepción si la respuesta HTTP no es exitosa
- **Qué se envía:** Código 403 (Forbidden).
- **Qué se espera:** Se lanza `HttpCoreException` con `ErrorType = Forbidden`.

### Caso 5: JSON inválido lanza HttpCoreException
- **Qué se envía:** Texto que no es JSON.
- **Qué se espera:** Se lanza `HttpCoreException` con `ErrorType = InternalServerError`.

---

## Clase: HttpCoreException

### Caso 1: Constructor con todos los parámetros
- **Qué hace:** Asigna correctamente `Key`, `Reason`, `StatusCode`, `ErrorMessage` y `ErrorType`.

### Caso 2: Constructor con valores nulos
- **Qué se envía:** `null` en todos los campos salvo `StatusCode`.
- **Qué se espera:** Valores por defecto como `"Unknown Key"`.

### Caso 3: Herencia
- **Qué se espera:** `HttpCoreException` debe heredar de `ResultException`.

---

## Clase: HttpCoreErrorEnum

### Caso 1: Cada enumerador tiene valor entero correcto
- **Qué hace:** Verifica que `BadRequest = 400`, `Unauthorized = 401`, etc.
- **Qué se envía:** Comparación entre enum y valor esperado.
- **Qué se espera:** Coincidencia exacta.

---

## Clase: Encryptions

### Caso 1: CreateMD5 devuelve hash esperado
- **Qué hace:** Calcula un hash MD5 para una cadena dada.
- **Qué se envía:** Texto plano.
- **Qué se espera:** Un hash hexadecimal de 32 caracteres.

### Caso 2: Base64Encode y Base64Decode son inversos
- **Qué se envía:** Texto plano codificado.
- **Qué se espera:** El resultado tras codificar y decodificar debe ser igual al original.

### Caso 3: SHA1, SHA256, SHA384, SHA512 devuelven hash esperado
- **Qué se envía:** Texto plano.
- **Qué se espera:** Códigos hash en formatos hexadecimales válidos.

---

## Clase: FileManagement

### Caso 1: Escribe y lee contenido desde archivo
- **Qué se envía:** Texto escrito en archivo temporal.
- **Qué se espera:** Lectura idéntica al texto original.

### Caso 2: Elimina archivos y verifica existencia
- **Qué se envía:** Ruta de archivo.
- **Qué se espera:** El archivo ya no debe existir.

---

## Clase: ResultException

### Caso 1: Constructor asigna valores correctamente
- **Qué se envía:** Parámetros como `Key`, `Reason`, `HttpStatusCode`.
- **Qué se espera:** Que las propiedades se asignen correctamente.

---

## Clase: ServiceResponse

### Caso 1: CreateSuccess y CreateError devuelven objeto esperado
- **Qué se envía:** Código de respuesta, mensaje de error y advertencias.
- **Qué se espera:** Propiedades `Success`, `ErrorMessage`, `WarningMessages` correctamente asignadas.

---

## Clase: ServiceResponse&lt;T&gt;

### Caso 1: CreateSuccess y CreateError devuelven datos genéricos correctamente
- **Qué se envía:** Objeto genérico + metadatos.
- **Qué se espera:** Datos en propiedad `Data` y `Success` reflejado correctamente.
