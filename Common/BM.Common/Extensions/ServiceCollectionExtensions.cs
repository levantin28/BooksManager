using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BM.Common.CQRS.Queries.Handler;
using BM.Common.CQRS.Commands.Dispatcher;
using BM.Common.CQRS.Queries.Dispatcher;
using BM.Common.CQRS.Commands.Handler;
using BM.Common.CQRS.Commands;
using BM.Common.CQRS.Queries;
using BM.Common.CQRS.Commands.Validator;
using BM.Common.Constants;
using System.Reflection;
using BM.Common.Infrastructure.Services;

namespace BM.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IFileService, FileService>();
        }
        // Adds services related to the Business Logic Layer (BLL).
        public static void AddBLL(this IServiceCollection services, Assembly assembly)
        {
            // Adds dispatchers, command handlers, command validators, and query handlers.
            services.AddDispatchers();
            services.AddCommandHandlers(assembly);
            services.AddCommandValidators(assembly);
            services.AddQueryHandlers(assembly);
        }

        // Adds command and query dispatchers as transient services.
        public static void AddDispatchers(this IServiceCollection services)
        {
            services.AddTransient<ICommandDispatcher, CommandDispatcher>();
            services.AddTransient<IQueryDispatcher, QueryDispatcher>();
        }

        // Adds command handlers based on the provided assembly.
        public static void AddCommandHandlers(this IServiceCollection services, Assembly assembly)
        {
            services.RegisterCommandServices(assembly, typeof(ICommandHandler<>));
        }

        // Adds query handlers based on the provided assembly.
        public static void AddQueryHandlers(this IServiceCollection services, Assembly assembly)
        {
            services.RegisterQueryServices(assembly, typeof(IQueryHandler<,>));
        }

        // Adds command validators based on the provided assembly.
        public static void AddCommandValidators(this IServiceCollection services, Assembly assembly)
        {
            services.RegisterCommandServices(assembly, typeof(ICommandValidator<>));
        }

        private static void RegisterCommandServices(this IServiceCollection services, Assembly assembly, Type type)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == type));

            // Loop through the handler types and register them as transient
            foreach (var handlerType in handlerTypes)
            {
                var genericInterface = handlerType.GetInterfaces()
                    .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == type);

                services.AddTransient(genericInterface, handlerType);
            }
        }

        private static void RegisterQueryServices(this IServiceCollection services, Assembly assembly, Type type)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == type));

            // Loop through the handler types and register them as transient
            foreach (var handlerType in handlerTypes)
            {
                var genericInterface = handlerType.GetInterfaces()
                    .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == type);

                var queryType = genericInterface.GetGenericArguments().First(); // Get the TQuery type from the generic interface
                var resultType = handlerType.GetInterfaces()
                    .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                    .GetGenericArguments().Last(); // Get the TResult type from the query handler's interface

                services.AddTransient(genericInterface, handlerType);
            }
        }


        // Adds Swagger generation services.
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
        }

        // Adds custom health checks.
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();

            // Adds a self-check that always returns a healthy result.
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            return services;
        }

        // Runs database migrations for a specified DbContext.
        public static void RunMigrations<TContext>(this IServiceProvider provider)
            where TContext : DbContext
        {
            using var scope = provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<TContext>();

            // Executes database migrations for the specified DbContext.
            context.Database.Migrate();
        }

        // Adds an Entity Framework DbContext with SQL Server as the data store.
        public static void AddBMDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext, new()
        {
            // Configures the DbContext to use SQL Server with connection string from configuration.
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(configuration.GetValue<string>(EnvironmentVariableConstants.DbConnectionString)));
        }
    }
}
