using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListsQueryHandler : IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>
    {
        public Task<Option<IEnumerable<ListDTO>, OperationFailure>> Execute(GetListsQuery query)
        {
            return Task.FromResult(
                Option.Some<IEnumerable<ListDTO>, OperationFailure>(new[] {new ListDTO {Id = "id", Name = "name"}})
            );
        }
    }
}