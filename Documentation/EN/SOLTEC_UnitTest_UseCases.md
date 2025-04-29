
# Use Cases - SOLTEC.Core Project - Unit Tests (English)

This document lists all use cases covered by unit tests implemented in the SOLTEC.Core project. It includes tests for the following classes:

- `HttpCore`
- `HttpCoreException`
- `HttpCoreErrorEnum`

---

## Class: HttpCore

### Case 1: GetAsync<T> returns a deserialized object
- **What it does:** Simulates a GET request and returns the deserialized object.
- **Input:** Simulated URL and JSON.
- **Expected:** Result object is not null and contains expected values.

### Case 2: PostAsync<T, TResult> returns a deserialized response
- **What it does:** Simulates a POST request with serialized body.
- **Expected:** Response contains expected data.

### Case 3: ValidateResult throws exception if ProblemDetailsDto indicates error
- **Input:** JSON with status 400.
- **Expected:** `HttpCoreException` is thrown.

### Case 4: ValidateStatusResponse throws exception on non-success HTTP code
- **Input:** HTTP 403 response.
- **Expected:** `HttpCoreException` with `ErrorType = Forbidden`.

### Case 5: Invalid JSON throws technical error
- **Input:** Malformed JSON string.
- **Expected:** `HttpCoreException` with `ErrorType = InternalServerError`.

---

## Class: HttpCoreException

### Case 1: Constructor with all parameters
- **What it does:** Assigns `Key`, `Reason`, `StatusCode`, `ErrorMessage`, `ErrorType`.

### Case 2: Constructor with null values
- **Input:** null in all parameters except `StatusCode`.
- **Expected:** Default values like `"Unknown Key"`.

### Case 3: Inheritance
- **Expected:** Inherits from `ResultException`.

---

## Class: HttpCoreErrorEnum

### Case 1: Enum values match HTTP codes
- **What it does:** Verifies each enum (e.g. `BadRequest = 400`, etc.)
- **Input:** Enum and expected int.
- **Expected:** Match exactly.

---

## Class: Encryptions

### Case 1: CreateMD5 returns expected hash
- **Input:** Plain text string.
- **Expected:** A 32-character hexadecimal MD5 hash.

### Case 2: Base64Encode and Base64Decode are inverses
- **Input:** Encoded plain text.
- **Expected:** Original text is recovered after encode and decode.

### Case 3: SHA1, SHA256, SHA384, SHA512 return expected hashes
- **Input:** Plain text.
- **Expected:** Valid hexadecimal string for each algorithm.

---

## Class: FileManagement

### Case 1: Write and read content from file
- **Input:** Text written to a temporary file.
- **Expected:** Read content matches original text.

### Case 2: Delete file and confirm removal
- **Input:** Path to a test file.
- **Expected:** File should no longer exist after deletion.

---

## Class: ResultException

### Case 1: Constructor assigns values properly
- **Input:** Parameters such as `Key`, `Reason`, `HttpStatusCode`.
- **Expected:** Properties are correctly set on the exception.

---

## Class: ServiceResponse

### Case 1: CreateSuccess and CreateError return correct structure
- **Input:** Response code, error message, warnings.
- **Expected:** Correct assignment to `Success`, `ErrorMessage`, `WarningMessages`.

---

## Class: ServiceResponse&lt;T&gt;

### Case 1: CreateSuccess and CreateError include typed data
- **Input:** Generic object and metadata.
- **Expected:** Object appears in `Data`, and `Success` is correctly reflected.
