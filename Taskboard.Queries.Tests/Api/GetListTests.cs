using System;
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
    public class GetListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var handler = new Mock<IQueryHandler<GetListQuery, ListDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListQuery>()))
                .ReturnsAsync(Option.Some<ListDTO, OperationFailure>(new ListDTO {Id = id, Name = "name"}));
            container.RegisterInstance(handler.Object);
            GetList.Container = container;

            var result = await GetList.Run(request, id, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as ListDTO;

            Assert.IsNotNull(value);

            Assert.AreEqual(id, value.Id);
            Assert.AreEqual("name", value.Name);
        }
    }
}