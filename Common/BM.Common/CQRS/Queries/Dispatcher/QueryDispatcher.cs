using Microsoft.Extensions.DependencyInjection;
using BM.Common.CQRS.Commands.Validator;
using BM.Common.CQRS.Queries.Handler;
using BM.Common.Models.Validation;

namespace BM.Common.CQRS.Queries.Dispatcher
{
    public class QueryDispatcher : IQueryDispatcher
    {
        // Field to store an instance of the service provider.
        private readonly IServiceProvider _serviceProvider;

        // Constructor to initialize the dispatcher with the necessary dependency.
        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Method to dispatch a command asynchronously and return a validation result.
        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : BMQuery<TResult>
        {
            // Retrieve the appropriate command handler and validator from the service provider.
            var queryHandler = _serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();

            return await queryHandler.HandleAsync(query);
        }
    }
}
