using System.Threading.Tasks;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListQueryHandler : IQueryHandler<GetListQuery, ListDTO>
    {
        public Task<ListDTO> Execute(GetListQuery query)
        {
            return Task.FromResult(new ListDTO {Id = query.Id, Name = "name"});
        }
    }
}