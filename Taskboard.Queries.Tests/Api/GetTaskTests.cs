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
    public class GetTaskTests
    {
        private static readonly TelemetryClient _telemetryClient = new TelemetryClient(new TelemetryConfiguration
        {
            DisableTelemetry = true
        });

        [TestMethod]
        public async Task Run_ReturnsTaskOnSuccess()
        {
            var handler = new Mock<IQueryHandler<GetTaskQuery, TaskDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var listid = Guid.NewGuid().ToString();
            var taskid = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetTaskQuery>()))
                .ReturnsAsync(new TaskDTO {Id = taskid, Name = "name", Description = "description"});
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            GetTask.Container = container;

            var result = await GetTask.Run(request, listid, taskid, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as TaskDTO;

            Assert.IsNotNull(value);

            Assert.AreEqual(taskid, value.Id);
            Assert.AreEqual("name", value.Name);
            Assert.AreEqual("description", value.Description);
        }

        [TestMethod]
        public async Task Run_ReturnsNotFoundOnNotFound()
        {
            var handler = new Mock<IQueryHandler<GetTaskQuery, TaskDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var listid = Guid.NewGuid().ToString();
            var taskid = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetTaskQuery>()))
                .ThrowsAsync(ResourceNotFoundException.FromResourceId(taskid));
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            GetTask.Container = container;

            var result = await GetTask.Run(request, listid, taskid, logger) as NotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Run_ReturnsErrorOnServerError()
        {
            var handler = new Mock<IQueryHandler<GetTaskQuery, TaskDTO>>();
            var container = new Container();
            var logger = new Mock<ILogger>().Object;
            var listid = Guid.NewGuid().ToString();
            var taskid = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            handler.Setup(h => h.Execute(It.IsAny<GetTaskQuery>()))
                .ThrowsAsync(new Exception());
            container.RegisterInstance(handler.Object);
            container.RegisterInstance(_telemetryClient);
            GetTask.Container = container;

            var result = await GetTask.Run(request, listid, taskid, logger) as InternalServerErrorResult;

            Assert.IsNotNull(result);
        }
    }
}