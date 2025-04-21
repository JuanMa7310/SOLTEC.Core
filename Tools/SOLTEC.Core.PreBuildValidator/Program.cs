/// <summary>
/// Entry point for the SOLTEC.Core.PreBuildValidator tool.
/// This tool performs pre-build checks such as LangVersion, Nullable, XML documentation, TODO/FIXME comments,
/// and unit test coverage validation for the SOLTEC.Core project.
/// </summary>
/// <example>
/// <code>
/// // Run from terminal
/// dotnet run --project Tools/SOLTEC.Core.PreBuildValidator
/// </code>
/// </example>
using SOLTEC.Core.PreBuildValidator.Validators;

Console.OutputEncoding = System.Text.Encoding.UTF8;

/// <summary>
/// Global variable: root directory of the solution.
/// </summary>
// 🌐 Global solution directory
var gsolutionDirectory = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE")
                        ?? Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));

/// <summary>
/// Global flag indicating overall validation success.
/// </summary>
bool gsuccess = true;

Console.WriteLine("🔍 Starting project validation......");

try
{
    /// <summary>
    /// Validates that all .csproj files have the correct LangVersion and nullable settings.
    /// </summary>
    /// <example>
    /// <code>
    /// LangVersionValidator.ValidateLangVersion(gsolutionDirectory);
    /// </code>
    /// </example>
    Console.WriteLine("📄 Checking LangVersion and Nullable in project...");
    LangVersionValidator.ValidateLangVersion(gsolutionDirectory);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ LangVersion or Nullable validation failed: {ex.Message}");
    Console.ResetColor();
    gsuccess = false;
}
try
{
    /// <summary>
    /// Validates that all public classes and members have XML documentation.
    /// </summary>
    /// <example>
    /// <code>
    /// XmlDocValidator.ValidateXmlDocumentation(gsolutionDirectory);
    /// </code>
    /// </example>
    Console.WriteLine("📝 Checking XML documentation...");
    XmlDocValidator.ValidateXmlDocumentation(gsolutionDirectory);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ XML documentation validation failed: {ex.Message}");
    Console.ResetColor();
    gsuccess = false;
}

try
{
    /// <summary>
    /// Validates the absence of TODO or FIXME comments in the source code.
    /// </summary>
    /// <example>
    /// <code>
    /// TodoFixmeValidator.ValidateTodoFixme(gsolutionDirectory);
    /// </code>
    /// </example>
    Console.WriteLine("🧠 Checking TODO / FIXME...");
    TodoFixmeValidator.ValidateTodoFixme(gsolutionDirectory);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ TODO/FIXME validation failed: {ex.Message}");
    Console.ResetColor();
    gsuccess = false;
}

try
{
    /// <summary>
    /// Validates that each logic-exposing class has a corresponding unit test class.
    /// </summary>
    /// <example>
    /// <code>
    /// TestCoverageValidator.ValidateTestCoverage(gsolutionDirectory);
    /// </code>
    /// </example>
    Console.WriteLine("📊 Checking test coverage by class...");
    TestCoverageValidator.ValidateTestCoverage(gsolutionDirectory);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ Test coverage validation failed: {ex.Message}");
    Console.ResetColor();
    gsuccess = false;
}

try
{
    /// <summary>
    /// Validates that unit testing projects contain test classes and required test methods 
    /// ([Fact] or [Test]) inside unit test classes.
    /// </summary>
    /// <example>
    /// <code>
    /// TestMethodPresenceValidator.ValidateTestMethods("path/to/solution");
    /// </code>
    /// </example>
    Console.WriteLine("🧪 Checking whether tests exist in unit testing projects...");
    TestMethodPresenceValidator.ValidateTestMethods(gsolutionDirectory);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ Test coverage validation failed: {ex.Message}");
    Console.ResetColor();
    gsuccess = false;
}

/// <summary>
/// Final result of the validations.
/// </summary>
if (gsuccess)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("✅ All validations passed successfully.");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("❌ Validation failed. Please fix the issues above.");
    Environment.Exit(1);
}
Console.ResetColor();
