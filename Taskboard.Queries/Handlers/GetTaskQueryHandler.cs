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
    public class GetTaskQueryHandler : QueryHandler<GetTaskQuery, TaskDTO>
    {
        public GetTaskQueryHandler(IDocumentClient documentClient, string db, string collection) :
            base(documentClient, db, collection)
        {
        }


        protected override async Task<TaskDTO> InternalExecute(GetTaskQuery query)
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
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw ResourceNotFoundException.FromResourceId(query.ListId);
            }
        }
    }
}