using System.Threading.Tasks;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListQueryHandler : IQueryHandler<GetListQuery, ListDTO>
    {
        public Task<Option<ListDTO, OperationFailure>> Execute(GetListQuery query)
        {
            return Task.FromResult(Option.Some<ListDTO, OperationFailure>(new ListDTO {Id = query.Id, Name = "name"}));
        }
    }
}