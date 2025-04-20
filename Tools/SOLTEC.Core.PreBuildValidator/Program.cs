/// <summary>
/// PreBuild Validator for the SOLTEC.Core solution.
/// This script performs pre-build validations for LangVersion, Nullable, XML docs, TODOs/FIXMEs, and test coverage.
/// </summary>
/// <example>
/// Run this file using:
/// <code>
/// dotnet run --project Tools/SOLTEC.Core.PreBuildValidator
/// </code>
/// </example>

using System.Text.RegularExpressions;
using System.Xml.Linq;

/// <summary>
/// Project validator runner.
/// </summary>
var gsolutionDirectory = "../";

Console.WriteLine("🔍 Starting project validation......");

/// <summary>
/// Validates LangVersion and Nullable setting in all csproj files.
/// </summary>
var gcsprojFiles = Directory.GetFiles(gsolutionDirectory, "*.csproj", SearchOption.AllDirectories);
foreach (var _file in gcsprojFiles)
{
    Console.WriteLine($"📝 Checking LangVersion and Nullable in project: {_file}...");
    var _xml = XDocument.Load(_file);
    var _props = _xml.Descendants("PropertyGroup");

    var _lang = _props.Elements("LangVersion").FirstOrDefault()?.Value;
    var _nullable = _props.Elements("Nullable").FirstOrDefault()?.Value;

    if (_lang != "12.0")
        Console.WriteLine($"❌ {_file}: LangVersion must be 12.0 (actual: {_lang ?? "NO DEFINIDO"})");

    if (_nullable != "enable")
        Console.WriteLine($"❌ {_file}: Nullable must be 'enable' (actual: {_nullable ?? "NO DEFINIDO"})");
}

/// <summary>
/// Validates that all public classes, enums, and interfaces contain XML documentation.
/// </summary>
var gcsFiles = Directory.GetFiles(gsolutionDirectory, "*.cs", SearchOption.AllDirectories);
foreach (var _file in gcsFiles.Where(f => !f.Contains("obj")))
{
    var _lines = File.ReadAllLines(_file);
    for (int _i = 0; _i < _lines.Length; _i++)
    {
        if (_lines[_i].Contains("public class") || _lines[_i].Contains("public enum") || _lines[_i].Contains("public interface"))
        {
            var _hasDoc = _i > 0 && _lines[_i - 1].TrimStart().StartsWith("///");
            if (!_hasDoc)
            {
                Console.WriteLine($"❌ {_file}: Public class missing XML documentation at line {_i + 1}");
            }
        }
    }
}

/// <summary>
/// Detects any TODO or FIXME tags in source code.
/// </summary>
Console.WriteLine("🔍 Checking TODO / FIXME...");
foreach (var _file in gcsFiles)
{
    var _lines = File.ReadAllLines(_file);
    for (int _i = 0; _i < _lines.Length; _i++)
    {
        if (_lines[_i].Contains("TODO") || _lines[_i].Contains("FIXME"))
            Console.WriteLine($"⚠️ {_file}: Pending comment on line {_i + 1}: {_lines[_i].Trim()}");
    }
}

/// <summary>
/// Confirms existence of at least one [Fact] or [Test] in unit testing projects.
/// </summary>
Console.WriteLine("🔍 Checking whether tests exist in unit testing projects...");
var _testMethodsFound = gcsFiles.Any(f =>
    f.Contains("Tests/") &&
    File.ReadAllText(f).Contains("[Fact]") || File.ReadAllText(f).Contains("[Test]"));

if (!_testMethodsFound)
{
    Console.WriteLine("❌ No test methods found using [Fact] or [Test]");
    Environment.Exit(1);
}

/// <summary>
/// Validates test class coverage for each logic class in SOLTEC.Core.
/// </summary>
Console.WriteLine("🔍 Checking test coverage by class...");
var _logicClasses = gcsFiles
    .Where(f => f.Contains("SOLTEC.Core/") && !f.Contains("Tests/") && !f.Contains("obj"))
    .ToList();

var _testFiles = gcsFiles
    .Where(f => f.Contains("Tests/") && !f.Contains("obj"))
    .ToList();

foreach (var _file in _logicClasses)
{
    var _fileName = Path.GetFileNameWithoutExtension(_file);
    var _expectedTest = _testFiles.FirstOrDefault(tf => Path.GetFileNameWithoutExtension(tf).Contains(_fileName));
    if (_expectedTest == null)
        Console.WriteLine($"❌ Missing unit test class for: {_fileName}");
    else
        Console.WriteLine($"✅ Found test class with test method: {Path.GetFileNameWithoutExtension(_expectedTest)}");
}

Console.WriteLine("✅ Validation complete.");
