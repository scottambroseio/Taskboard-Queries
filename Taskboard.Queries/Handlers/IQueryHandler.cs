using System.Threading.Tasks;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> Execute(TQuery query);
    }
}