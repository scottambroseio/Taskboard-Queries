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
    public static class GetList
    {
        public static Container Container = BuildContainer();

        [FunctionName(nameof(GetList))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "list/{id}")] HttpRequest req, string id,
            ILogger log)
        {
            var query = new GetListQuery {Id = id};
            var handler = Container.GetInstance<IQueryHandler<GetListQuery, ListDTO>>();

            var result = await handler.Execute(query);

            return result.Match<IActionResult>(
                content => new OkObjectResult(content),
                error => new InternalServerErrorResult()
            );
        }

        private static Container BuildContainer()
        {
            var container = new Container();

            container.Register<IQueryHandler<GetListQuery, ListDTO>, GetListQueryHandler>();

            return container;
        }
    }
}