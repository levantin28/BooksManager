using BM.Common.CQRS.Commands.Handler;
using BM.Common.CQRS.Commands.Validator;
using BM.Common.Models.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace BM.Common.CQRS.Commands.Dispatcher
{
    public class CommandDispatcher : ICommandDispatcher
    {
        // Field to store an instance of the service provider.
        private readonly IServiceProvider _serviceProvider;

        // Constructor to initialize the dispatcher with the necessary dependency.
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Method to dispatch a command asynchronously and return a validation result.
        public async Task<ValidationResultModel> DispatchAsync<TCommand>(TCommand command) where TCommand : BMCommand
        {
            // Retrieve the appropriate command handler and validator from the service provider.
            var commandHandler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
            var commandValidator = _serviceProvider.GetService<ICommandValidator<TCommand>>();

            // Validate the command if a validator is available.

            if (commandValidator != null)
            {
                // Validate the command using the command validator.
                var result = await commandValidator.ValidateAsync(command);

                // Check if the validation result indicates that the command is not valid.
                if (result is { IsValid: false })
                {
                    // Return the validation result immediately.
                    return result;
                }
            }

            // Execute the command handler to perform the associated action.
            await commandHandler.HandleAsync(command);

            // Return an empty validation result, indicating successful command execution.
            return new ValidationResultModel();
        }
    }
}
