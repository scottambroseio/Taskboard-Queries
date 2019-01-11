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
    public class GetListQueryHandlerTests
    {
        [TestMethod]
        public async Task Execute_ReturnsListOnSuccess()
        {
            var repo = new Mock<IListRepository>();
            var query = new GetListQuery {Id = "id"};
            var handler = new GetListQueryHandler(repo.Object);
            var expected = new ListDTO();

            repo.Setup(r => r.GetById(query.Id)).ReturnsAsync(Option.Some<ListDTO, CosmosFailure>(expected));

            var result = await handler.Execute(query);

            result.Match(
                list => Assert.AreEqual(expected, list),
                failure => Assert.Fail()
            );
        }

        [TestMethod]
        public async Task Execute_ReturnsErrorOnFailure()
        {
            var repo = new Mock<IListRepository>();
            var query = new GetListQuery {Id = "id"};
            var handler = new GetListQueryHandler(repo.Object);

            repo.Setup(r => r.GetById(query.Id)).ReturnsAsync(Option.None<ListDTO, CosmosFailure>(CosmosFailure.Error));

            var result = await handler.Execute(query);

            result.Match(
                list => Assert.Fail(),
                error => Assert.AreEqual(QueryFailure.Error, error)
            );
        }

        [TestMethod]
        public async Task Execute_ReturnsNotFoundOnNotFound()
        {
            var repo = new Mock<IListRepository>();
            var query = new GetListQuery {Id = "id"};
            var handler = new GetListQueryHandler(repo.Object);

            repo.Setup(r => r.GetById(query.Id))
                .ReturnsAsync(Option.None<ListDTO, CosmosFailure>(CosmosFailure.NotFound));

            var result = await handler.Execute(query);

            result.Match(
                list => Assert.Fail(),
                error => Assert.AreEqual(QueryFailure.NotFound, error)
            );
        }
    }
}