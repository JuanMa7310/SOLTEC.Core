using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

var gSourcePath = @"C:\SOLTEC\Source\SOLTEC.Core\SOLTEC.Core";
var gOutputPathEn = Path.Combine(Directory.GetCurrentDirectory(), "Docs", "en");
var gOutputPathEs = Path.Combine(Directory.GetCurrentDirectory(), "Docs", "es");

Directory.CreateDirectory(gOutputPathEn);
Directory.CreateDirectory(gOutputPathEs);

Console.WriteLine($"> Searching in: {gSourcePath}");

foreach (var _sourceFile in Directory.GetFiles(gSourcePath, "*.cs", SearchOption.AllDirectories))
{
    var _fileContent = await File.ReadAllTextAsync(_sourceFile);
    var _syntaxTree = CSharpSyntaxTree.ParseText(_fileContent);
    var _syntaxRoot = await _syntaxTree.GetRootAsync();

    var _classDeclarations = _syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>()
        .Where(c => c.Modifiers.Any(SyntaxKind.PublicKeyword));

    var _enumDeclarations = _syntaxRoot.DescendantNodes().OfType<EnumDeclarationSyntax>()
        .Where(e => e.Modifiers.Any(SyntaxKind.PublicKeyword));

    foreach (var _classDeclaration in _classDeclarations)
    {
        var _className = _classDeclaration.Identifier.Text;
        var _classDoc = ExtractXmlDoc(_classDeclaration);
        var _classSummary = _classDoc?.Split('.').FirstOrDefault()?.Trim() ?? "No summary available.";

        var _classMdEn = new StringBuilder();
        var _classMdEs = new StringBuilder();

        _classMdEn.AppendLine($"# 📦 {_className}\n\n> {_classSummary}\n\n## Public Members\n");
        _classMdEs.AppendLine($"# 📦 {_className}\n\n> {Translate(_classSummary)}\n\n## Miembros Públicos\n");

        foreach (var _member in _classDeclaration.Members.Where(m => m.Modifiers.Any(SyntaxKind.PublicKeyword)))
        {
            var _signature = _member switch
            {
                MethodDeclarationSyntax m => m.Identifier.Text + m.ParameterList.ToString(),
                PropertyDeclarationSyntax p => p.Identifier.Text,
                ConstructorDeclarationSyntax c => c.Identifier.Text + c.ParameterList.ToString(),
                _ => _member.ToString()
            };

            var _memberDoc = ExtractXmlDoc(_member);
            _classMdEn.AppendLine($"- `{_signature}` — {_memberDoc ?? "No XML documentation."}");
            _classMdEs.AppendLine($"- `{_signature}` — {Translate(_memberDoc) ?? "Sin documentación XML."}");
        }

        _classMdEn.AppendLine($"\n---\n[Ver en Español](https://github.com/JuanMa7310/SOLTEC.Core/wiki/{_className}_ES)");
        _classMdEs.AppendLine($"\n---\n[View in English](https://github.com/JuanMa7310/SOLTEC.Core/wiki/{_className})");

        await File.WriteAllTextAsync(Path.Combine(gOutputPathEn, $"{_className}.md"), _classMdEn.ToString());
        await File.WriteAllTextAsync(Path.Combine(gOutputPathEs, $"{_className}_ES.md"), _classMdEs.ToString());
    }

    foreach (var _enumDeclaration in _enumDeclarations)
    {
        var _enumMdEn = new StringBuilder();
        var _enumMdEs = new StringBuilder();
        var _enumName = _enumDeclaration.Identifier.Text;
        var _enumSummary = ExtractXmlDoc(_enumDeclaration);
        var _enumDoc = ExtractXmlDoc(_enumDeclaration);

        _enumMdEn.AppendLine($"# 🧷 {_enumName}\n\n> {_enumSummary}\n\n## Enum Members\n");
        _enumMdEs.AppendLine($"# 🧷 {_enumName}\n\n> {Translate(_enumSummary)}\n\n## Miembros del Enumerado\n");

        foreach (var _member in _enumDeclaration.Members)
        {
            var _memberDoc = ExtractXmlDoc(_member);
            _enumMdEn.AppendLine($"- `{_member.Identifier}` — {_memberDoc ?? "No XML documentation."}");
            _enumMdEs.AppendLine($"- `{_member.Identifier}` — {Translate(_memberDoc) ?? "Sin documentación XML."}");
        }

        _enumMdEn.AppendLine($"\n---\n[Ver en Español](https://github.com/JuanMa7310/SOLTEC.Core/wiki/{_enumName}_ES)");
        _enumMdEs.AppendLine($"\n---\n[View in English](https://github.com/JuanMa7310/SOLTEC.Core/wiki/{_enumName})");

        await File.WriteAllTextAsync(Path.Combine(gOutputPathEn, $"{_enumName}.md"), _enumMdEn.ToString());
        await File.WriteAllTextAsync(Path.Combine(gOutputPathEs, $"{_enumName}_ES.md"), _enumMdEs.ToString());

}
    }

// TOC & HOME GENERATION
var _githubBaseUrl = "https://github.com/JuanMa7310/SOLTEC.Core/wiki";

var _enFiles = Directory.GetFiles(gOutputPathEn, "*.md")
    .Where(f => !Path.GetFileName(f).StartsWith("Home") && !Path.GetFileName(f).StartsWith("TOC"))
    .OrderBy(f => f);

var _esFiles = Directory.GetFiles(gOutputPathEs, "*.md")
    .Where(f => !Path.GetFileName(f).StartsWith("Home") && !Path.GetFileName(f).StartsWith("TOC"))
    .OrderBy(f => f);

var _tocEn = "# Table of Contents\n\n" + string.Join("\n", _enFiles.Select(f =>
{
    var _page = Path.GetFileNameWithoutExtension(f);
    return $"- [{_page}]({_githubBaseUrl}/{_page})";
}));

var _tocEs = "# Índice de Contenidos\n\n" + string.Join("\n", _esFiles.Select(f =>
{
    var _page = Path.GetFileNameWithoutExtension(f);
    return $"- [{_page.Replace("_ES", "")}]({_githubBaseUrl}/{_page})";
}));

var _homeEn = "# 🌐 SOLTEC.Core Wiki (English)\n" +
              "Welcome to the official documentation for **SOLTEC.Core**, a .NET library that provides utilities for secure HTTP communication, file management, data encryption, response standardization, and more.\n" +
              "---\n" +
              "## 📌 Quick Start\n" +
              "- [📚 View All Components](TOC)\n" +
              "- 🇪🇸 [View this page in Spanish](Home_ES)\n" +
              "---\n" +
              "## 🧩 What You'll Find Here\n" +
              "- 📦 Public classes with IntelliSense-ready XML comments\n" +
              "- 🧪 Unit-tested methods with xUnit and NUnit\n" +
              "- 🔐 Custom exception handling\n" +
              "- 🧰 Helpers for cryptography, HTTP, and validation\n" +
              "---\n" +
              "## 🛠️ Current Version: 1.0.0\n" +
              "Check the [Changelog](https://github.com/JuanMa7310/SOLTEC.Core/wiki/CHANGELOG) or [Features](https://github.com/JuanMa7310/SOLTEC.Core/wiki/FEATURES) for full capabilities.\n";

var _homeEs = "# 🌐 Wiki de SOLTEC.Core (Español)\n" +
              "Bienvenido a la documentación oficial de **SOLTEC.Core**, una librería en .NET que proporciona utilidades para comunicación HTTP segura, manejo de archivos, encriptación de datos, estandarización de respuestas, y más.\n" +
              "---\n" +
              "## 📌 Inicio Rápido\n" +
              "- [📚 Ver todos los componentes](TOC_ES)\n" +
              "- 🇬🇧 [View this page in English](Home)\n" +
              "---\n" +
              "## 🧩 ¿Qué encontrarás aquí?\n" +
              "- 📦 Clases públicas con comentarios XML compatibles con IntelliSense\n" +
              "- 🧪 Métodos verificados con pruebas unitarias en xUnit y NUnit\n" +
              "- 🔐 Manejadores personalizados de excepciones\n" +
              "- 🧰 Herramientas de cifrado, HTTP y validación\n" +
              "---\n" +
              "## 🛠️ Versión actual: 1.0.0\n" +
              "Consulta el [Registro de Cambios](https://github.com/JuanMa7310/SOLTEC.Core/wiki/CHANGELOG_ES) o las [Características](https://github.com/JuanMa7310/SOLTEC.Core/wiki/FEATURES_ES) para conocer todas las capacidades.\n";

await File.WriteAllTextAsync(Path.Combine(gOutputPathEn, "Home.md"), _homeEn);
await File.WriteAllTextAsync(Path.Combine(gOutputPathEs, "Home_ES.md"), _homeEs);
await File.WriteAllTextAsync(Path.Combine(gOutputPathEn, "TOC.md"), _tocEn);
await File.WriteAllTextAsync(Path.Combine(gOutputPathEs, "TOC_ES.md"), _tocEs);

Console.WriteLine("✅ Wiki Markdown generation completed.");

// Helper methods
static string? ExtractXmlDoc(SyntaxNode node)
{
    var trivia = node.GetLeadingTrivia()
                    .FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                                         t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
    if (trivia == default) return null;

    var xml = trivia.ToFullString();
    return string.Join(" ", xml.Replace("///", "").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()));
}
static string Translate(string? input)
{
    if (string.IsNullOrWhiteSpace(input)) return "Sin traducción disponible.";
    return "[ES] " + input;
}