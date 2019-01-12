using System;
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
using Taskboard.Queries.Exceptions;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Tests.Api
{
    [TestClass]
    public class GetListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsListOnSuccess()
        {
            var handler = new Mock<IQueryHandler<GetListQuery, ListDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var client = new TelemetryClient(new TelemetryConfiguration
            {
                DisableTelemetry = true
            });
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListQuery>())).ReturnsAsync(new ListDTO {Id = id, Name = "name"});
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(client);
            GetList.Container = container;

            var result = await GetList.Run(request, id, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as ListDTO;

            Assert.IsNotNull(value);

            Assert.AreEqual(id, value.Id);
            Assert.AreEqual("name", value.Name);
        }

        [TestMethod]
        public async Task ValidRequest_ReturnsNotFoundOnNotFound()
        {
            var handler = new Mock<IQueryHandler<GetListQuery, ListDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var client = new TelemetryClient(new TelemetryConfiguration
            {
                DisableTelemetry = true
            });
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListQuery>()))
                .ThrowsAsync(ResourceNotFoundException.FromResourceId(id));
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(client);
            GetList.Container = container;

            var result = await GetList.Run(request, id, logger) as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ValidRequest_ReturnsServerErrorOnError()
        {
            var handler = new Mock<IQueryHandler<GetListQuery, ListDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var client = new TelemetryClient(new TelemetryConfiguration
            {
                DisableTelemetry = true
            });
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetListQuery>()))
                .ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(client);
            GetList.Container = container;

            var result = await GetList.Run(request, id, logger) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}