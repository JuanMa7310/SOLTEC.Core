# ✅ CODING RULES for SOLTEC Projects

## 🔧 General Rules
1. **Language**: C# version 12  
2. **Framework**: .NET 8  
3. **Version control**: Git (hosted on GitHub)  

## 🏷️ Naming Conventions

1. **Global class-level variables** (private, public, or protected) must start with lowercase `g` followed by a lowercase letter.
   - Example: `gcontext`, `ghttpClient`, `glogger`.

2. **Local variables** inside methods or functions must start with an underscore `_` followed by a lowercase letter.
   - Example: `_path`, `_index`, `_list`.

3. **Inline declared variables** must also start with `_` followed by a lowercase letter.
   - Example: `foreach (var _item in _collection)`.

4. **Lambda expression variables** must:
   - Use a lowercase letter representing the model.
   - If the model has a compound name with an uppercase second part, the second letter must be the lowercase version of that uppercase.
   - Examples:
     - `Nomina`: `n`
     - `NominaDetalle`: `nd`
     - `ServiceResponse`: `sr`
     - `InvoiceType`: `it`

   **Recommended usage**:
   ```csharp
   _listNominas.Select(n => n.Name);
   _listNominaDetalle.Select(nd => nd.Date);
   _serviceResponses.Select(sr => sr.Success);
   ```

5. **Global constants** must start with `gc` followed by a lowercase letter.
   - Example: `gcTimeout`, `gcPath`, `gcHeader`.

6. **Local constants** must start with `_c` followed by a lowercase letter.
   - Example: `_cMaxSize`, `_cPrefix`.

## 📘 XML Documentation

1. All **public classes** must include **XML comments in English**.

2. All **public methods and functions** must include:
   - XML comments in English.
   - At least one usage example inside `<![CDATA[ ]]>` (⚠️ do not use `<code>`).
   ```csharp
   /// <summary>
   /// Reverses a string.
   /// </summary>
   /// <param name="input">The string to reverse.</param>
   /// <returns>The reversed string.</returns>
   /// <example>
   /// <![CDATA[
   /// var reversed = StringUtils.Reverse("abc"); // "cba"
   /// ]]>
   /// </example>
   ```

3. All **public enumerations (`enum`)** must:
    - Have XML comments in English.
    - Provide a brief summary describing the enum purpose.

## ✅ Compatibility

1. All XML code examples must use `<![CDATA[ ]]>` to ensure compatibility with GitHub Actions and automatic documentation tools.

## 🧾 Documentation

1. All **public or protected classes, methods, functions, properties, and fields** must include XML documentation in English.

2. XML documentation for **classes and methods** must contain **code usage examples**.

3. XML documentation examples are **exempt** from naming rules.

## 🧪 Testing Guidelines

1. All public or protected methods and functions must be covered by **unit tests** and **integration tests**, both in **xUnit** and **NUnit** frameworks.

2. **Unit test methods** must include XML comments describing what is being tested, what is sent, and what is expected.

3. Classes without any exposed logic (no public/protected methods or functions) **do not require tests**.

## 🌍 Language

1. All externally visible text strings must be written in **English**.