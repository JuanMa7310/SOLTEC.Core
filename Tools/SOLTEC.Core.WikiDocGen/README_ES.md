# 📘 SOLTEC.Core.WikiDocGen - Guía de Uso

Este proyecto genera automáticamente documentación Markdown bilingüe (inglés y español) para todas las clases públicas, propiedades, métodos y enumeraciones del proyecto `SOLTEC.Core`.

## 🚀 Cómo Ejecutar

Asegúrate de compilar el proyecto antes de ejecutar el generador.

### ✅ Desde la línea de comandos de .NET
```bash
dotnet run --project ./Tools/SOLTEC.Core.WikiDocGen
```

### ✅ Desde Windows Terminal (con script de ayuda)
```bash
./Tools/SOLTEC.Core.WikiDocGen/run-wiki-generator.bat
```

### ✅ Desde Linux o terminal de Rider
```bash
sh ./Tools/SOLTEC.Core.WikiDocGen/run-wiki-generator.sh
```

## 📂 Salida

Los archivos Markdown generados se ubican en:
```
./Tools/SOLTEC.Core.WikiDocGen/DOCS/
```

## 🌐 Integración con la Wiki

Copia el contenido de `DOCS/en/` y `DOCS/es/` en el repositorio Wiki de GitHub, ya sea con `git clone` o manualmente.

---

Los archivos generados incluyen:
- `Home.md`, `Home_ES.md`
- `TOC.md`, `TOC_ES.md`
- Un archivo Markdown por cada clase o enumeración (con ejemplos y comentarios XML)
