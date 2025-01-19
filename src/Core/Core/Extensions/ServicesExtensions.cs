using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddNamedHttpClient(this IServiceCollection services, string baseAddress, string clientName)
        {
            services.AddHttpClient(clientName, client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        }
    }
}
