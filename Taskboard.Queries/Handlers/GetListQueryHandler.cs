using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Exceptions;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListQueryHandler : IQueryHandler<GetListQuery, ListDTO>
    {
        private readonly string collection;
        private readonly string db;
        private readonly IDocumentClient documentClient;

        public GetListQueryHandler(IDocumentClient documentClient, string db,
            string collection)
        {
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public async Task<ListDTO> Execute(GetListQuery query)
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
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw ResourceNotFoundException.FromResourceId(query.Id);
                }

                throw DataAccessException.FromInnerException(ex);
            }
        }
    }
}