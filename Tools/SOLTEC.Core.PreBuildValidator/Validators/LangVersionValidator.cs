using System.Xml.Linq;

namespace SOLTEC.Core.PreBuildValidator.Validators;

/// <summary>
/// Validates that each project (.csproj) has the correct language version and nullable settings enabled.
/// </summary>
/// <example>
/// <code>
/// LangVersionValidator.ValidateLangVersion("../MySolution");
/// </code>
/// </example>
public static class LangVersionValidator
{
    /// <summary>
    /// The required C# language version.
    /// </summary>
    private const string gRequiredLangVersion = "12.0";

    /// <summary>
    /// Validates that all .csproj files in the solution have LangVersion set to 12.0 and Nullable enabled.
    /// </summary>
    /// <param name="gsolutionDirectory">Path to the solution directory to scan.</param>
    public static void ValidateLangVersion(string gsolutionDirectory)
    {
        Console.WriteLine("🔍 Starting Checking LangVersion and Nullable in project...");

        var _projectFiles = Directory.GetFiles(gsolutionDirectory, "*.csproj", SearchOption.AllDirectories);

        foreach (var _file in _projectFiles)
        {
            Console.WriteLine($"📝 Checking LangVersion and Nullable in project: {_file}...");

            try
            {
                var _xdoc = XDocument.Load(_file);
                var _ns = _xdoc.Root?.Name.Namespace;

                var _langVersion = _xdoc.Descendants(_ns + "LangVersion").FirstOrDefault()?.Value?.Trim();
                var _nullable = _xdoc.Descendants(_ns + "Nullable").FirstOrDefault()?.Value?.Trim();

                if (_langVersion != gRequiredLangVersion)
                {
                    Console.WriteLine($"❌ {_file}: LangVersion must be {gRequiredLangVersion} (actual: {_langVersion ?? "NOT DEFINED"})");
                }

                if (_nullable?.ToLowerInvariant() != "enable")
                {
                    Console.WriteLine($"❌ {_file}: Nullable must be enabled (actual: {_nullable ?? "NOT DEFINED"})");
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"❌ Error parsing {_file}: {_ex.Message}");
            }
        }
    }
}