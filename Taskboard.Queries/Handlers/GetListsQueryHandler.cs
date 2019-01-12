using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public class GetListsQueryHandler : QueryHandler<GetListsQuery, IEnumerable<ListDTO>>
    {
        public GetListsQueryHandler(IDocumentClient documentClient, string db, string collection) :
            base(documentClient, db, collection)
        {
        }

        protected override Task<IEnumerable<ListDTO>> InternalExecute(GetListsQuery query)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(db, collection);

            return Task.FromResult(documentClient.CreateDocumentQuery<ListDTO>(uri).AsEnumerable());
        }
    }
}