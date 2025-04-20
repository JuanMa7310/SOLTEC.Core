// See https://aka.ms/new-console-template for more information
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("🔍 Starting project validation......");

var gsolutionDirectory = Path.Combine("..", "SOLTEC.Core");
var gprojects = new[]
{
    "SOLTEC.Core",
    "Tests/SOLTEC.Core.Tests.NuNit",
    "Tests/SOLTEC.Core.Tests.xUnit",
    "Tools/SOLTEC.Core.PreBuildValidator",
    "Tools/SOLTEC.Core.WikiDocGen"
};

foreach (var _project in gprojects)
{
    Console.WriteLine($"📝 Checking LangVersion and Nullable in project: {Path.Combine(gsolutionDirectory, _project)}...");

    var _csprojPath = Path.Combine(gsolutionDirectory, _project, $"{Path.GetFileNameWithoutExtension(_project)}.csproj");
    var _csprojContent = File.ReadAllText(_csprojPath);

    if (!_csprojContent.Contains("<LangVersion>12.0</LangVersion>"))
        Console.WriteLine($"❌ {_csprojPath}: LangVersion must be 12.0 (actual: NO DEFINIDO)");

    if (!_csprojContent.Contains("<Nullable>enable</Nullable>"))
        Console.WriteLine($"❌ {_csprojPath}: Nullable must be enabled");
}

// Ejecutar las validaciones
XmlDocValidator.ValidateXmlDocumentation(gsolutionDirectory);
TestCoverageValidator.ValidateTestCoverage(gsolutionDirectory);
TestMethodPresenceValidator.ValidateTestMethods(gsolutionDirectory);

/// <summary>
/// Provides methods to check and validate XML documentation in C# files.
/// </summary>
public static class XmlDocValidator
{
    public static void ValidateXmlDocumentation(string gsolutionDirectory)
    {
        Console.WriteLine("📝 Checking XML documentation...");

        var _csFiles = Directory.GetFiles(gsolutionDirectory, "*.cs", SearchOption.AllDirectories)
                                .Where(f => !f.Contains(@"\obj\") && !f.Contains(@"\bin\"))
                                .ToList();

        foreach (var _file in _csFiles)
        {
            Console.WriteLine($"📝 Checking XML documentation in file: {_file}...");

            var _lines = File.ReadAllLines(_file);
            for (int _i = 0; _i < _lines.Length; _i++)
            {
                if (_lines[_i].Trim().StartsWith("public class") ||
                    _lines[_i].Trim().StartsWith("public static class") ||
                    _lines[_i].Trim().StartsWith("public record") ||
                    _lines[_i].Trim().StartsWith("public interface") ||
                    _lines[_i].Trim().StartsWith("public enum"))
                {
                    var _hasXmlComment = (_i > 0 && _lines[_i - 1].Trim().StartsWith("///"));
                    if (!_hasXmlComment)
                    {
                        Console.WriteLine($"❌ {_file}: Public class missing XML documentation en la línea {_i + 1}");
                    }
                }
            }
        }
    }
}

/// <summary>
/// Validates that each public class in the main project has a corresponding unit test class.
/// </summary>
public static class TestCoverageValidator
{
    public static void ValidateTestCoverage(string gsolutionDirectory)
    {
        Console.WriteLine("🔍 Checking whether tests exist in unit testing projects...");
        Console.WriteLine("🔍 Checking test coverage by class...");

        var _logicClasses = Directory.GetFiles(gsolutionDirectory, "*.cs", SearchOption.AllDirectories)
            .Where(f => f.Contains("SOLTEC.Core/") && !f.Contains("Tests") && !f.Contains("obj") && !f.Contains("bin"))
            .ToList();

        foreach (var _logicClass in _logicClasses)
        {
            var _className = Path.GetFileNameWithoutExtension(_logicClass);
            var _testProjectsPath = new[]
            {
                Path.Combine(gsolutionDirectory, "Tests", "SOLTEC.Core.Tests.NuNit"),
                Path.Combine(gsolutionDirectory, "Tests", "SOLTEC.Core.Tests.xUnit")
            };

            Console.WriteLine($"📝 Checking if a unit test class exists for the file: {_logicClass}...");

            bool _found = _testProjectsPath.Any(testProjectPath =>
                Directory.GetFiles(testProjectPath, "*.cs", SearchOption.AllDirectories)
                        .Any(testFile =>
                        {
                            var _content = File.ReadAllText(testFile);
                            return _content.Contains(_className) && _content.Contains("[Test") || _content.Contains("[Fact");
                        }));

            if (_found)
                Console.WriteLine($"✅ Found test class with test method: {_className}Tests");
            else
                Console.WriteLine($"❌ Missing unit test class for: {_className}Tests");
        }
    }
}

/// <summary>
/// Contains the logic to verify that test classes include actual test methods.
/// </summary>
public static class TestMethodPresenceValidator
{
    public static void ValidateTestMethods(string gsolutionDirectory)
    {
        Console.WriteLine("🔍 Checking if unit test classes contain test methods...");

        var _testProjectsPath = new[]
        {
            Path.Combine(gsolutionDirectory, "Tests", "SOLTEC.Core.Tests.NuNit"),
            Path.Combine(gsolutionDirectory, "Tests", "SOLTEC.Core.Tests.xUnit")
        };

        foreach (var _testProjectPath in _testProjectsPath)
        {
            var _testFiles = Directory.GetFiles(_testProjectPath, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains("obj") && !f.Contains("bin"))
                .ToList();

            foreach (var _file in _testFiles)
            {
                var _content = File.ReadAllText(_file);
                if (!_content.Contains("[Test") && !_content.Contains("[Fact"))
                {
                    Console.WriteLine($"❌ {_file}: No test methods found.");
                }
            }
        }
    }
}
