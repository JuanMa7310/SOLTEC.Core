using System.Text.RegularExpressions;
using System.Xml.Linq;

var _csprojFiles = Directory.GetFiles("..", "*.csproj", SearchOption.AllDirectories);
var _csFiles = Directory.GetFiles("..", "*.cs", SearchOption.AllDirectories);
bool _hasError = false;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("🔍 Starting project validation......");
// ✅ Validate csproj: LangVersion and Nullable
foreach (var _file in _csprojFiles)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"📝 Checking {_file}...");
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
foreach (var _file in _csFiles.Where(f => f.Contains("SOLTEC.Core/")))
{
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
foreach (var _file in _csFiles.Where(f => f.Contains("SOLTEC.Core/")))
{
    var _lines = File.ReadAllLines(_file);
    for (int i = 0; i < _lines.Length; i++)
    {
        if (_lines[i].Contains("TODO") || _lines[i].Contains("FIXME"))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  { _file}: Pending comment on line { i + 1}: { _lines[i].Trim()}");
            Console.ResetColor();
            _hasError = true;
        }
    }
}

// ✅ Validate that there are test methods in test projects
var _testFiles = _csFiles.Where(f => f.ToLower().Contains("test"));
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

if (_hasError)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ Validation failed. Please fix the errors before building.");
    Console.ResetColor();
    Environment.Exit(1);
}
else
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✅ All validations passed successfully.");
    Console.ResetColor();
}