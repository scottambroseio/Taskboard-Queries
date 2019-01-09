using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Queries.Api;

namespace Taskboard.Queries.Tests.Api
{
    [TestClass]
    public class GetListTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var logger = new Mock<ILogger>().Object;
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            var result = await GetList.Run(request, id, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as ListDTO;

            Assert.IsNotNull(value);

            Assert.AreEqual(id, value.Id);
            Assert.AreEqual("name", value.Name);
        }
    }
}
