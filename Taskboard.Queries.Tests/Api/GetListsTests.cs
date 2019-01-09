using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetListsTests
    {
        [TestMethod]
        public void ValidRequest_ReturnsCorrectResponse()
        {
            var logger = new Mock<ILogger>().Object;
            var id = Guid.NewGuid().ToString();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            var result = GetLists.Run(request, logger) as OkObjectResult;

            Assert.IsNotNull(result);

            var value = result.Value as IEnumerable<ListDTO>;

            Assert.IsNotNull(value);

            var list = value.Single();

            Assert.AreEqual("id", list.Id);
            Assert.AreEqual("name", list.Name);
        }
    }
}
