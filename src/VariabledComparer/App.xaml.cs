using System.Reflection;
using System.Windows;

namespace VariabledComparer;

public sealed partial class App : Application
{
    public static string InformationalVersion { get; } = typeof(App).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;
}
