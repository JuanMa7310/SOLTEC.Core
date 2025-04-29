# ✅ NORMAS DE PROGRAMACIÓN para Proyectos SOLTEC

## 🔧 Reglas Generales
1. **Lenguaje**: C# versión 12  
2. **Framework**: .NET 8  
3. **Control de versiones**: Git (alojado en GitHub)  

## 🏷️ Convenciones de Nombres

1. Las **variables globales** a nivel de clase (privadas, públicas o protegidas) deben comenzar con `g` minúscula seguida de minúscula.
   - Ejemplo: `gcontext`, `ghttpClient`, `glogger`.

2. Las **variables locales** dentro de métodos o funciones deben comenzar con un guion bajo `_` seguido de minúscula.
   - Ejemplo: `_path`, `_index`, `_list`.

3. Las **variables declaradas inline** también deben iniciar con `_` seguido de minúscula.
   - Ejemplo: `foreach (var _item in _collection)`.

4. Las **variables de expresiones lambda** deben:
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
5. Las **constantes globales** deben comenzar con `gc` seguidas de una letra minúscula.
   - Ejemplo: `gcTimeout`, `gcPath`, `gcHeader`.

6. Las **constantes locales** deben comenzar con `_c` seguidas de una letra minúscula.
   - Ejemplo: `_cMaxSize`, `_cPrefix`.

## 📘 Documentación XML

1. Todas las **clases públicas** deben tener **comentarios XML en inglés**.

2. Todos los **métodos y funciones públicas** deben incluir:
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

3. Todos los **enumerados públicos (`enum`)** deben:
    - Tener comentarios XML en inglés.
    - Incluir un resumen corto describiendo el propósito del enum.

## ✅ Compatibilidad

1. Todos los ejemplos en comentarios XML deben usar `<![CDATA[ ]]>` para asegurar compatibilidad con GitHub Actions y herramientas de documentación automática.

## 🧾 Documentación

1. Todas las **clases, métodos, funciones, propiedades y campos públicos o protegidos** deben incluir documentación XML en inglés.

2. La documentación XML de **clases y métodos** debe contener **ejemplos de uso en código**.

3. Las variables utilizadas dentro de ejemplos de documentación XML **están exentas** de las reglas de nomenclatura.

## 🧪 Guías de Pruebas

1. Todos los métodos y funciones públicos o protegidos deben estar cubiertos por **pruebas unitarias** y **pruebas de integración**, tanto con **xUnit** como con **NUnit**.

2. Los **métodos de prueba unitaria** deben incluir comentarios XML que describan qué se prueba, qué se envía y qué se espera.

3. Las clases que no tengan lógica expuesta (sin métodos o funciones públicas/protegidas) **no requieren pruebas**.

## 🌍 Idioma

1. Todas las cadenas de texto visibles externamente deben estar escritas en **inglés**.