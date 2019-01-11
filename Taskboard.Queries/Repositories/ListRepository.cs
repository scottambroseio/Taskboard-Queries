using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Optional;
using Taskboard.Queries.DTO;
using Taskboard.Queries.Enums;

namespace Taskboard.Queries.Repositories
{
    public class ListRepository : IListRepository
    {
        private readonly string collection;
        private readonly string db;
        private readonly IDocumentClient documentClient;
        private readonly TelemetryClient telemetryClient;

        public ListRepository(TelemetryClient telemetryClient, IDocumentClient documentClient, string db,
            string collection)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public async Task<Option<ListDTO, CosmosFailure>> GetById(string id)
        {
            try
            {
                var uri = UriFactory.CreateDocumentUri(db, collection, id);

                var document = await documentClient.ReadDocumentAsync<ListDTO>(uri, new RequestOptions
                {
                    PartitionKey = new PartitionKey(id)
                });

                return Option.Some<ListDTO, CosmosFailure>(document);
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return Option.None<ListDTO, CosmosFailure>(CosmosFailure.NotFound);
                }

                telemetryClient.TrackException(ex);

                return Option.None<ListDTO, CosmosFailure>(CosmosFailure.Error);
            }
        }

        public Task<Option<IEnumerable<ListDTO>, CosmosFailure>> GetAll()
        {
            try
            {
                var uri = UriFactory.CreateDocumentCollectionUri(db, collection);

                var documents = documentClient.CreateDocumentQuery<ListDTO>(uri).AsEnumerable();

                return Task.FromResult(Option.Some<IEnumerable<ListDTO>, CosmosFailure>(documents));
            }
            catch (DocumentClientException ex)
            {
                telemetryClient.TrackException(ex);

                return Task.FromResult(Option.None<IEnumerable<ListDTO>, CosmosFailure>(CosmosFailure.Error));
            }
        }
    }
}