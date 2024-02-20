using BM.Common.Models.Validation;

namespace BM.Common.CQRS.Commands.Validator
{
    public abstract class CommandValidator<TCommand> : ICommandValidator<TCommand>
        where TCommand : BMCommand
    {
        public abstract Task<ValidationResultModel> ValidateAsync(TCommand command);
    }
}
