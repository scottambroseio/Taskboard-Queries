using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Exceptions;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListQueryHandler : QueryHandler<GetListQuery, ListDTO>
    {
        public GetListQueryHandler(IDocumentClient documentClient, string db, string collection) :
            base(documentClient, db, collection)
        {
        }

        protected override async Task<ListDTO> InternalExecute(GetListQuery query)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, query.Id);

                var document = await documentClient.ReadDocumentAsync<ListDTO>(uri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(query.Id)
                });

                return document.Document;
            }
            catch (DocumentClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw ResourceNotFoundException.FromResourceId(query.Id);
            }
        }
    }
}