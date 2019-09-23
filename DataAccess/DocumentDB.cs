using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DataAccess
{
  public class DocumentDB<T> : IStorage<T> where T : class
  {
    private readonly string _endpoint;
    private readonly string _key;
    private readonly string _databaseId;
    private readonly string _collectionId;
    private DocumentClient _client;

    public DocumentDB(string endpoint, string key, string databaseId, string collectionId)
    {
      this._endpoint = endpoint;
      this._key = key;
      this._databaseId = databaseId;
      this._collectionId = collectionId;
      this._client = new DocumentClient(new Uri(_endpoint), _key);
      CreateDatabaseIfNotExistsAsync().Wait();
      CreateCollectionIfNotExistsAsync().Wait();
    }

    public async Task<T> GetItemAsync(string id)
    {
      try
      {
        Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
        return (T)(dynamic)document;
      }
      catch (DocumentClientException e)
      {
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          return null;
        }
        else
        {
          throw;
        }
      }
    }

    public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
    {
      IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
          UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
          new FeedOptions { MaxItemCount = -1 })
          .Where(predicate)
          .AsDocumentQuery();

      List<T> results = new List<T>();
      while (query.HasMoreResults)
      {
        results.AddRange(await query.ExecuteNextAsync<T>());
      }

      return results;
    }

    public async Task<IEnumerable<T>> GetItemsAsync()
    {
      IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
          UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
          new FeedOptions { MaxItemCount = -1 })
          .AsDocumentQuery();

      List<T> results = new List<T>();
      while (query.HasMoreResults)
      {
        results.AddRange(await query.ExecuteNextAsync<T>());
      }

      return results;
    }

    public async Task<Document> CreateItemAsync(T item)
    {
      return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
    }

    public async Task<Document> UpdateItemAsync(string id, T item)
    {
      return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), item);
    }

    public async Task DeleteItemAsync(string id)
    {
      await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
    }

    private async Task CreateDatabaseIfNotExistsAsync()
    {
      // TODO: Create db with shared 400 RU/s across all containers
      try
      {
        await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
      }
      catch (DocumentClientException e)
      {
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          await _client.CreateDatabaseAsync(new Database { Id = _databaseId });
        }
        else
        {
          throw;
        }
      }
    }

    private async Task CreateCollectionIfNotExistsAsync()
    {
      // TODO: Add email unqiueness constraint on members collection
      try
      {
        await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
      }
      catch (DocumentClientException e)
      {
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound && _collectionId == "members")
        {
          await _client.CreateDocumentCollectionAsync(
            UriFactory.CreateDatabaseUri(_databaseId),
            new DocumentCollection
            {
              Id = _collectionId,
              UniqueKeyPolicy = new UniqueKeyPolicy
              {
                UniqueKeys =
                  new Collection<UniqueKey>
                  {
                      new UniqueKey { Paths = new Collection<string> { "/email" }},
                  }
              }
            },
            new RequestOptions { OfferThroughput = 400 });
        }
        else if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          await _client.CreateDocumentCollectionAsync(
            UriFactory.CreateDatabaseUri(_databaseId),
            new DocumentCollection
            {
              Id = _collectionId,
            },
            new RequestOptions { OfferThroughput = 400 });
        }
        else
        {
          throw;
        }
      }
    }
  }
}
