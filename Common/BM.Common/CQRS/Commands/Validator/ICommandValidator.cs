using BM.Common.Models.Validation;

namespace BM.Common.CQRS.Commands.Validator
{
    public interface ICommandValidator<TCommand>
        where TCommand : BMCommand
    {
        Task<ValidationResultModel> ValidateAsync(TCommand command);
    }
}
