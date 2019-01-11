using System.Threading.Tasks;
using Optional;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<Option<TResult, QueryFailure>> Execute(TQuery query);
    }
}