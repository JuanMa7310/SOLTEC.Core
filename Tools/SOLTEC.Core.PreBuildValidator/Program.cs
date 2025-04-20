using System.Text.RegularExpressions;
using System.Xml.Linq;

var _csprojFiles = Directory.GetFiles("..", "*.csproj", SearchOption.AllDirectories);
var _csFiles = Directory.GetFiles("..", "*.cs", SearchOption.AllDirectories);
bool _hasError = false;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("🔍 Starting project validation......");
Console.ResetColor();
// ✅ Validate csproj: LangVersion and Nullable
foreach (var _file in _csprojFiles)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"📝 Checking LangVersion and Nullable in project: {_file}...");
    Console.ResetColor();

    try
    {
        var _xml = XDocument.Load(_file);
        var _langVersion = _xml.Descendants("LangVersion").FirstOrDefault()?.Value;
        var _nullable = _xml.Descendants("Nullable").FirstOrDefault()?.Value;

        if (_langVersion != "12.0")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {_file}: LangVersion must be 12.0 (actual: {_langVersion ?? "NO DEFINIDO"})");
            _hasError = true;
        }

        if (_nullable?.ToLowerInvariant() != "enable")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {_file}: Nullable must be enabled (actual: {_nullable ?? "NO DEFINIDO"})");
            _hasError = true;
        }

        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"🔥 Error reading {_file}: {ex.Message}");
        Console.ResetColor();
        _hasError = true;
    }
}

// ✅ Validate public classes have XML documentation
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"📝 Checking XML documentation...");
Console.ResetColor();
foreach (var _file in _csFiles.Where(f => f.Replace('\\', '/').Contains("SOLTEC.Core/")))
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"📝 Checking XML documentation in file: {_file}...");
    Console.ResetColor();

    var _lines = File.ReadAllLines(_file);
    for (int i = 1; i < _lines.Length; i++)
    {
        if (_lines[i].Contains("public class") && !_lines[i - 1].Trim().StartsWith("///"))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {_file}: Public class missing XML documentation en la línea {i + 1}");
            Console.ResetColor();
            _hasError = true;
        }
    }
}

// ✅ Check TODO / FIXME
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("🔍 Checking TODO / FIXME...");
Console.ResetColor();
foreach (var _file in _csFiles.Where(f => f.Replace('\\', '/').Contains("SOLTEC.Core/")))
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"📝 Checking TODO/FIXME in file: {_file}...");
    Console.ResetColor();

    var _lines = File.ReadAllLines(_file);
    for (int i = 0; i < _lines.Length; i++)
    {
        if (_lines[i].Contains("TODO") || _lines[i].Contains("FIXME"))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  {_file}: Pending comment on line {i + 1}: {_lines[i].Trim()}");
            Console.ResetColor();
            _hasError = true;
        }
    }
}

// ✅ Validate that there are test methods in test projects
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("🔍 Checking whether tests exist in unit testing projects...");
Console.ResetColor();
var _testFiles = _csFiles.Where(f => f.Contains("test", StringComparison.CurrentCultureIgnoreCase));
bool _hasTests = _testFiles.Any(f =>
{
    var content = File.ReadAllText(f);
    return content.Contains("[Fact]") || content.Contains("[Test]");
});
if (!_hasTests)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ No test methods with [Fact] or [Test] were found con [Fact] o [Test].");
    Console.ResetColor();
    _hasError = true;
}

// ✅ Validate each public class in SOLTEC.Core has a matching test class
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("🔍 Checking test coverage by class...");
Console.ResetColor();

var _logicFiles = _csFiles
    .Where(f => f.Replace('\\', '/').Contains("SOLTEC.Core/"))
    .Where(f => f.EndsWith(".cs") && File.ReadAllText(f).Contains("public class"))
    .Where(f =>
{
var _content = File.ReadAllText(f);
var _className = Path.GetFileNameWithoutExtension(f);

Console.WriteLine($"➡️ Reviewing: {_className}");

var excludedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ProblemDetailsDto"
        };

if (excludedNames.Contains(_className))
{
Console.WriteLine($"⛔ Excluded by name list: {_className}");
return false;
}

bool hasPublicMethod = Regex.IsMatch(_content, @"public\s+(static\s+)?[\w\<\>\[\]]+\s+\w+\s*\(");
bool hasConstructor = Regex.IsMatch(_content, $@"public\s+{_className}\s*\(");
bool hasAssignmentsInConstructor = _content.Contains(" = ") && hasConstructor;
bool hasOverride = Regex.IsMatch(_content, @"public\s+override");
bool hasExpressionBody = _content.Contains("=>");
bool onlyProperties = Regex.IsMatch(_content, @"public\s+.*?{\s*get;\s*set;\s*}", RegexOptions.IgnoreCase);

bool hasLogic = hasPublicMethod || hasConstructor || hasOverride || hasExpressionBody || hasAssignmentsInConstructor;

if (hasLogic || !onlyProperties)
{
Console.ForegroundColor = ConsoleColor.Blue;
Console.WriteLine($"✅ Detected logic class: {_className}");
Console.ResetColor();
return true;
}

Console.WriteLine($"🚫 No logic detected in: {_className}");
return false;
})
    .ToList();

// ✅ Check test class and methods per logic class
_testFiles = _csFiles
    .Where(f => f.Replace('\\', '/').Contains("Tests/") && f.EndsWith(".cs"))
    .ToList()!;

foreach (var _file in _logicFiles)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"📝 Checking if a unit test class exists for the file: {_file}...");
    Console.ResetColor();

    var _name = Path.GetFileNameWithoutExtension(_file);
    var _expectedTestName = _name + "Tests";

    var _matchedTest = _testFiles.FirstOrDefault(t => Path.GetFileNameWithoutExtension(t).Equals(_expectedTestName, StringComparison.OrdinalIgnoreCase));
    if (_matchedTest == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Missing unit test class for: {_name}");
        // _testCoverageReport.Add($"| {_name} | ❌ | No test class found |");
        Console.ResetColor();
        _hasError = true;
    }
    else
    {
        if (!File.Exists(_matchedTest))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Test file not found: {_matchedTest}");
            Console.ResetColor();
            _hasError = true;
            continue;
        }

        var _testContent = File.ReadAllText(_matchedTest);
        if (_testContent.Contains("[Fact]") || _testContent.Contains("[Test]"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ Found test class with test method: {_expectedTestName}");
            // _testCoverageReport.Add($"| {_name} | ✅ | Tests present |");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  {_expectedTestName} exists but has no [Fact] or [Test] methods");
            // _testCoverageReport.Add($"| {_name} | ⚠️ | No test methods |");
            Console.ResetColor();
            _hasError = true;
        }
    }
}

if (_hasError)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ Validation failed. Please fix the issues above.");
    Console.ResetColor();
    Environment.Exit(1);
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✅ All validations passed successfully.");
    Console.ResetColor();
}