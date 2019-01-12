using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Taskboard.Queries.Exceptions;
using Taskboard.Queries.Queries;

namespace Taskboard.Queries.Handlers
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery
    {
        protected readonly string collection;
        protected readonly string db;
        protected readonly IDocumentClient documentClient;

        protected QueryHandler(IDocumentClient documentClient, string db, string collection)
        {
            this.documentClient = documentClient ?? throw new ArgumentNullException(nameof(documentClient));
            this.db = !string.IsNullOrWhiteSpace(db) ? db : throw new ArgumentNullException(nameof(db));
            this.collection = !string.IsNullOrWhiteSpace(collection)
                ? collection
                : throw new ArgumentNullException(nameof(collection));
        }

        public Task<TResult> Execute(TQuery query)
        {
            try
            {
                return InternalExecute(query);
            }
            catch (DocumentClientException ex)
            {
                throw DataAccessException.FromInnerException(ex);
            }
        }

        protected abstract Task<TResult> InternalExecute(TQuery query);
    }
}