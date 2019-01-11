using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Optional;
using SimpleInjector;
using Taskboard.Queries.Api;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Tests.Api
{
    [TestClass]
    public class GetListsTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListsQuery>()))
                .ReturnsAsync(
                    Option.Some<IEnumerable<ListDTO>, QueryFailure>(new[]
                        {new ListDTO {Id = "id", Name = "name"}}));
            container.RegisterInstance(handler.Object);
            GetLists.Container = container;

            var result = await GetLists.Run(request, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as IEnumerable<ListDTO>;

            Assert.IsNotNull(value);

            var list = value.Single();

            Assert.AreEqual("id", list.Id);
            Assert.AreEqual("name", list.Name);
        }
    }
}