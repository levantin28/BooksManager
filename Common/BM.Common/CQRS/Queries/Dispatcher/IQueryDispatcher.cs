using BM.Common.CQRS.Commands;
using BM.Common.Models.Validation;

namespace BM.Common.CQRS.Queries.Dispatcher
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : BMQuery<TResult>;
    }
}
