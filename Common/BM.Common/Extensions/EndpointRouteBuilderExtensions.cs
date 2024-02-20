using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;

namespace BM.Common.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapCustomHealthChecks(this IEndpointRouteBuilder endpoints, string route = "/hc")
        {
            endpoints.MapHealthChecks(route, new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
        }
    }
}
