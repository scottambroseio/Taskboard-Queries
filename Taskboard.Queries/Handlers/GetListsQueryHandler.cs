using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Exceptions;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListsQueryHandler : IQueryHandler<GetListsQuery, IEnumerable<ListDTO>>
    {
        private readonly string collection;
        private readonly string db;
        private readonly IDocumentClient documentClient;

        public GetListsQueryHandler(IDocumentClient documentClient, string db,
            string collection)
        {
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public Task<IEnumerable<ListDTO>> Execute(GetListsQuery query)
        {
            try
            {
                var uri = UriFactory.CreateDocumentCollectionUri(db, collection);

                return Task.FromResult(documentClient.CreateDocumentQuery<ListDTO>(uri).AsEnumerable());
            }
            catch (DocumentClientException ex)
            {
                throw DataAccessException.FromInnerException(ex);
            }
        }
    }
}