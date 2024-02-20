namespace BM.Common.CQRS.Commands.Handler
{
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : BMCommand
    {
        public abstract Task HandleAsync(TCommand command);
    }
}
