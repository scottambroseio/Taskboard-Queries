using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Api
{
    public static class GetList
    {
        [FunctionName(nameof(GetList))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "list/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            var query = new GetListQuery {Id = id};
            var handler = new GetListQueryHandler();

            var result = await handler.Execute(query);

            return new OkObjectResult(result);
        }
    }
}