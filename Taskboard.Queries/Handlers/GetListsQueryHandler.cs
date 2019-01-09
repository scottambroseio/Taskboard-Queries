using System.Collections.Generic;
using System.Threading.Tasks;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListsQueryHandler : IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>
    {
        public Task<IEnumerable<ListDTO>> Execute(GetListsQuery query)
        {
            return Task.FromResult((IEnumerable<ListDTO>) new[] {new ListDTO {Id = "id", Name = "name"}});
        }
    }
}