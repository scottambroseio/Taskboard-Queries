using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Taskboard.Queries.Api;
using Taskboard.Queries.DTO;

namespace Taskboard.Queries.Tests.Api
{
    [TestClass]
    public class GetListsTests
    {
        [TestMethod]
        public async Task ValidRequest_ReturnsCorrectResponse()
        {
            var logger = new Mock<ILogger>().Object;
            var request = new DefaultHttpRequest(new DefaultHttpContext());

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