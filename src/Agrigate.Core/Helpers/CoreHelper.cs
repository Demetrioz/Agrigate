using System.Reflection;

namespace Agrigate.Core.Helpers;

/// <summary>
/// A helper class containing common logic shared among different areas of Agrigate
/// </summary>
public static class CoreHelper
{
    /// <summary>
    /// Retrieves the namespace of the running project, falling back to a pre-defined default
    /// </summary>
    /// <returns></returns>
    public static string GetSourceNamespace()
    {
        return Assembly
                .GetEntryAssembly()?.EntryPoint?.DeclaringType?.Assembly.FullName
                ?.Split(",")
                .FirstOrDefault()
            ?? Constants.Logging.DefaultSource;
    }
}