using BM.Common.Models.Validation;

namespace BM.Common.CQRS.Commands.Dispatcher
{
    public interface ICommandDispatcher
    {
        Task<ValidationResultModel> DispatchAsync<TCommand>(TCommand command) where TCommand : BMCommand;
    }
}
