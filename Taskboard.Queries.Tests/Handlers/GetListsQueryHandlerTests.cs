using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;
using Taskboard.Queries.Repositories;

namespace Taskboard.Queries.Tests.Handlers
{
    [TestClass]
    public class GetListsQueryHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsListsOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var query = new GetListsQuery();
            var handler = new GetListsQueryHandler(repo.Object);
            var expected = new[] {new ListDTO()};

            repo.Setup(r => r.GetAll()).ReturnsAsync(Option.Some<IEnumerable<ListDTO>, CosmosFailure>(expected));

            var result = await handler.Execute(query);

            result.Match(
                lists => Assert.AreEqual(expected, lists),
                failure => Assert.Fail()
            );
        }

        [TestMethod]
        public async Task Execute_ReturnsErrorOnFailure()
        {
            var repo = new Mock<IListRepository>();
            var query = new GetListsQuery();
            var handler = new GetListsQueryHandler(repo.Object);

            repo.Setup(r => r.GetAll())
                .ReturnsAsync(Option.None<IEnumerable<ListDTO>, CosmosFailure>(CosmosFailure.Error));

            var result = await handler.Execute(query);

            result.Match(
                list => Assert.Fail(),
                error => Assert.AreEqual(QueryFailure.Error, error)
            );
        }
    }
}