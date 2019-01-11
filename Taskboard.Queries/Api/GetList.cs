using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;
using Taskboard.Queries.Repositories;

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
                error =>
                {
                    switch (error)
                    {
                        case QueryFailure.NotFound: return new NotFoundResult();
                        default: return new InternalServerErrorResult();
                    }
                }
            );
        }

        private static Container BuildContainer()
        {
            var container = new Container();

            container.RegisterSingleton(() => new TelemetryClient
            {
                InstrumentationKey = Environment.GetEnvironmentVariable("AI_INSTRUMENTATIONKEY")
            });
            container.RegisterSingleton<IDocumentClient>(() =>
                new DocumentClient(new Uri(Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")),
                    Environment.GetEnvironmentVariable("COSMOS_KEY")));
            container.Register<IListRepository>(() => new ListRepository(container.GetInstance<TelemetryClient>(),
                container.GetInstance<IDocumentClient>(),
                Environment.GetEnvironmentVariable("COSMOS_DB"),
                Environment.GetEnvironmentVariable("COSMOS_COLLECTION")));
            container.Register<IQueryHandler<GetListQuery, ListDTO>, GetListQueryHandler>();

            return container;
        }
    }
}