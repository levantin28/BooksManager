using Microsoft.AspNetCore.Builder;

namespace BM.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Object Manager API V1");
            });
        }
    }
}
