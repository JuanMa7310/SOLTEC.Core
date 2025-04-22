# ✅ NORMAS DE PROGRAMACIÓN para Proyectos SOLTEC

## 🔧 Reglas Generales
1. **Lenguaje**: C# versión 12  
2. **Framework**: .NET 8  
3. **Control de versiones**: Git (alojado en GitHub)  

## 🏷️ Convenciones de Nombres

4. Las **variables globales** a nivel de clase (privadas, públicas o protegidas) deben comenzar con `g` minúscula seguida de minúscula.
   - Ejemplo: `gcontext`, `ghttpClient`, `glogger`.

5. Las **variables locales** dentro de métodos o funciones deben comenzar con un guion bajo `_` seguido de minúscula.
   - Ejemplo: `_path`, `_index`, `_list`.

6. Las **variables declaradas inline** también deben iniciar con `_` seguido de minúscula.
   - Ejemplo: `foreach (var _item in _collection)`.

7. Las **variables de expresiones lambda** deben:
   - Ser letras en minúscula que representen el nombre del modelo.
   - Si el modelo tiene una segunda parte que empieza con mayúscula, la segunda letra debe ser esa mayúscula en minúscula.
   - Ejemplos:
     - `Nomina`: `n`
     - `NominaDetalle`: `nd`
     - `ServiceResponse`: `sr`
     - `InvoiceType`: `it`

   **Uso recomendado**:
   ```csharp
   _listNominas.Select(n => n.Nombre);
   _listNominaDetalle.Select(nd => nd.Fecha);
   _serviceResponses.Select(sr => sr.Success);
   ```

## 📘 Documentación XML

8. Todas las **clases públicas** deben tener **comentarios XML en inglés**.

9. Todos los **métodos y funciones públicas** deben incluir:
   - Comentarios XML en inglés.
   - Al menos un ejemplo de uso dentro de `<![CDATA[ ]]>` (⚠️ no usar `<code>`).
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

10. Todos los **enumerados públicos (`enum`)** deben:
    - Tener comentarios XML en inglés.
    - Incluir un resumen corto describiendo el propósito del enum.

## ✅ Compatibilidad

11. Todos los ejemplos en comentarios XML deben usar `<![CDATA[ ]]>` para asegurar compatibilidad con GitHub Actions y herramientas de documentación automática.