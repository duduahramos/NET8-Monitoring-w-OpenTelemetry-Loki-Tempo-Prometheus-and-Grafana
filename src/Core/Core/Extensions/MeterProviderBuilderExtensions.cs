using Core.Common;
using OpenTelemetry.Metrics;

namespace Core.Extensions
{
    public static class MeterProviderBuilderExtensions
    {
        public static MeterProviderBuilder AddCustomMeter(this MeterProviderBuilder builder, string applicationName)
        {
            var meterName = $"{applicationName}_metrics_custom_meter";
            builder.AddMeter(meterName);

            return builder;
        }
    }
}
