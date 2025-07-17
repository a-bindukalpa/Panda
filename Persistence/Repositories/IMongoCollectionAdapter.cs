using MongoDB.Driver;

namespace Persistence.Repositories;

public interface IMongoCollectionAdapter<T>
{
    Task<T?> FindOneAsync(FilterDefinition<T> filter);
    Task InsertOneAsync(T document);
    Task<bool> ReplaceOneAsync(FilterDefinition<T> filter, T document);
    Task<bool> DeleteOneAsync(FilterDefinition<T> filter);
}