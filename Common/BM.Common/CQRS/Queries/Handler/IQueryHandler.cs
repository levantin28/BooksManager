namespace BM.Common.CQRS.Queries.Handler
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : BMQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
