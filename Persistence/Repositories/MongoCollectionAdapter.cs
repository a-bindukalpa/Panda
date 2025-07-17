using MongoDB.Driver;

namespace Persistence.Repositories;

public class MongoCollectionAdapter<T>(IMongoCollection<T> col) : IMongoCollectionAdapter<T>
{
    public async Task<T?> FindOneAsync(FilterDefinition<T> filter) => await col.Find(filter).FirstOrDefaultAsync();

    public Task InsertOneAsync(T document) => col.InsertOneAsync(document);

    public async Task<bool> ReplaceOneAsync(FilterDefinition<T> filter, T document)
    {
        var result = await col.ReplaceOneAsync(filter, document);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteOneAsync(FilterDefinition<T> filter)
    {
        var result = await col.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}