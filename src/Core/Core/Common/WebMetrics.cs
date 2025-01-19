using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Metrics;

namespace Core.Common
{
    public class WebMetrics
    {
        private readonly string _meterName;
        private readonly Counter<long> _requestCounter;
        private readonly Histogram<long> _requestDuration;

        public WebMetrics(IMeterFactory meterFactory, string meterName)
        {
            _meterName = $"{meterName}_metrics_custom_meter";
            var meter = meterFactory.Create(_meterName);

            _requestCounter = meter.CreateCounter<long>($"{_meterName}.requests.count");

            _requestDuration = meter.CreateHistogram<long>($"{_meterName}.requests.duration", "ms");
        }
    }
}
