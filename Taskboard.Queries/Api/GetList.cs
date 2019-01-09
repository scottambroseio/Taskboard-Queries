using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Taskboard.Queries.Api
{
    public static class GetList
    {
        [FunctionName(nameof(GetList))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "list/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            var list = new ListDTO {Id = id, Name = "name"};

            return new OkObjectResult(list);
        }
    }
}