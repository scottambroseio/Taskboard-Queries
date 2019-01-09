using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Taskboard.Queries.Api
{
    public static class GetLists
    {
        [FunctionName(nameof(GetLists))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists")] HttpRequest req, ILogger log)
        {
            var list = new[] {new ListDTO {Id = "id", Name = "name"}};

            return new OkObjectResult(list);
        }
    }
}