namespace BM.Common.CQRS.Commands.Handler
{
    public interface ICommandHandler<TCommand>
        where TCommand : BMCommand
    {
        Task HandleAsync(TCommand command);
    }
}
