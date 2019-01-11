using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Queries;
using Taskboard.Queries.Repositories;

namespace Taskboard.Queries.Handlers
{
    public class GetListsQueryHandler : IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>
    {
        private readonly IListRepository repo;

        public GetListsQueryHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Option<IEnumerable<ListDTO>, QueryFailure>> Execute(GetListsQuery query)
        {
            var result = await repo.GetAll();

            return result.Match(
                lists => Option.Some<IEnumerable<ListDTO>, QueryFailure>(lists),
                failure => Option.None<IEnumerable<ListDTO>, QueryFailure>(QueryFailure.Error)
            );
        }
    }
}