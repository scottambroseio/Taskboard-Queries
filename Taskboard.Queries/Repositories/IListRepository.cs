using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;

namespace Taskboard.Queries.Repositories
{
    public interface IListRepository
    {
        Task<Option<ListDTO, CosmosFailure>> GetById(string id);
        Task<Option<IEnumerable<ListDTO>, CosmosFailure>> GetAll();
    }
}