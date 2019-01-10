using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Api
{
    public static class GetLists
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(GetLists))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "lists")] HttpRequest req, ILogger log)
        {
            var query = new GetListsQuery();
            var handler = new GetListsQueryHandler();

            var result = await handler.Execute(query);

            return result.Match<IActionResult>(
                content => new OkObjectResult(content),
                error => new InternalServerErrorResult()
            );
        }

        private static Container BuildContainer()
        {
            var container = new Container();

            container.Register<IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>, GetListsQueryHandler>();

            return container;
        }
    }
}