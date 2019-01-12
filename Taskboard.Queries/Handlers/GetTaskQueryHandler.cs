using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Exceptions;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetTaskQueryHandler : IQueryHandler<GetTaskQuery, TaskDTO>
    {
        private readonly string collection;
        private readonly string db;
        private readonly IDocumentClient documentClient;

        public GetTaskQueryHandler(IDocumentClient documentClient, string db,
            string collection)
        {
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public async Task<TaskDTO> Execute(GetTaskQuery query)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, query.ListId);

                var document = await documentClient.ReadDocumentAsync<ListDTO>(uri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(query.ListId)
                });

                var task = document.Document.Tasks.FirstOrDefault(t => t.Id == query.TaskId);

                if (task == null)
                {
                    throw ResourceNotFoundException.FromResourceId(query.TaskId);
                }

                return task;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw ResourceNotFoundException.FromResourceId(query.ListId);
                }

                throw DataAccessException.FromInnerException(ex);
            }
        }
    }
}