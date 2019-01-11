using System;
using System.Threading.Tasks;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Queries;
using Taskboard.Queries.Repositories;

namespace Taskboard.Queries.Handlers
{
    public class GetListQueryHandler : IQueryHandler<GetListQuery, ListDTO>
    {
        private readonly IListRepository repo;

        public GetListQueryHandler(IListRepository repo)
        {
            this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<Option<ListDTO, QueryFailure>> Execute(GetListQuery query)
        {
            var result = await repo.GetById(query.Id);

            return result.Match(
                list => Option.Some<ListDTO, QueryFailure>(list),
                failure =>
                {
                    switch (failure)
                    {
                        case CosmosFailure.NotFound:
                            return Option.None<ListDTO, QueryFailure>(QueryFailure.NotFound);
                        default:
                            return Option.None<ListDTO, QueryFailure>(QueryFailure.Error);
                    }
                });
        }
    }
}