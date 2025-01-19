using System.Diagnostics;

namespace Core;

public static class DiagnosticsConfig
{
    private const string ServiceName = "MyService";
    public static ActivitySource ActivitySource;

    public static string GetServiceName(string applicationName)
    {
        var serviceName = $"{ServiceName} - {applicationName}";

        ActivitySource = new ActivitySource(serviceName);

        return serviceName;
    }
}