using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;
using Taskboard.Queries.Api;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Tests.Api
{
    [TestClass]
    public class GetListsTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsListsOnSuccess()
        {
            var handler = new Mock<IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var client = new TelemetryClient(new TelemetryConfiguration
            {
                DisableTelemetry = true
            });
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListsQuery>()))
                .ReturnsAsync(new[] {new ListDTO {Id = "id", Name = "name"}});

            container.RegisterInstance(handler.Object);
            container.RegisterInstance(client);
            GetLists.Container = container;

            var result = await GetLists.Run(request, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as IEnumerable<ListDTO>;

            Assert.IsNotNull(value);

            var list = value.Single();

            Assert.AreEqual("id", list.Id);
            Assert.AreEqual("name", list.Name);
        }

        [TestMethod]
        public async Task ValidRequest_ReturnsServerErrorOnError()
        {
            var handler = new Mock<IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var client = new TelemetryClient(new TelemetryConfiguration
            {
                DisableTelemetry = true
            });
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListsQuery>()))
                .ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(client);
            GetLists.Container = container;

            var result = await GetLists.Run(request, logger) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}