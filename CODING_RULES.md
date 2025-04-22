# ✅ CODING RULES for SOLTEC Projects

## 🔧 General Rules
1. **Language**: C# version 12  
2. **Framework**: .NET 8  
3. **Version control**: Git (hosted on GitHub)  

## 🏷️ Naming Conventions

4. **Global class-level variables** (private, public, or protected) must start with lowercase `g` followed by a lowercase letter.
   - Example: `gcontext`, `ghttpClient`, `glogger`.

5. **Local variables** inside methods or functions must start with an underscore `_` followed by a lowercase letter.
   - Example: `_path`, `_index`, `_list`.

6. **Inline declared variables** must also start with `_` followed by a lowercase letter.
   - Example: `foreach (var _item in _collection)`.

7. **Lambda expression variables** must:
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

## 📘 XML Documentation

8. All **public classes** must include **XML comments in English**.

9. All **public methods and functions** must include:
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

10. All **public enumerations (`enum`)** must:
    - Have XML comments in English.
    - Provide a brief summary describing the enum purpose.

## ✅ Compatibility

11. All XML code examples must use `<![CDATA[ ]]>` to ensure compatibility with GitHub Actions and automatic documentation tools.