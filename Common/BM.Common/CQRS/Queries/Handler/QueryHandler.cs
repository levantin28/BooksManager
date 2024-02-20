namespace BM.Common.CQRS.Queries.Handler
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : BMQuery<TResult>
    {
        public abstract Task<TResult> HandleAsync(TQuery query);
    }
}
