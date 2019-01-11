using System;
using System.Collections.Generic;
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
using Taskboard.Queries.Handlers;
using Taskboard.Queries.Queries;
using Taskboard.Queries.Repositories;

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
            var handler = Container.GetInstance<IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>>();

            var result = await handler.Execute(query);

            return result.Match<IActionResult>(
                content => new OkObjectResult(content),
                error => new InternalServerErrorResult()
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
            container.Register<IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>, GetListsQueryHandler>();

            return container;
        }
    }
}