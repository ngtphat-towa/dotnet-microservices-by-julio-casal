using System.Data.Common;
using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Common.Entities;
namespace Play.Common.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IEntity
{

    private readonly IMongoCollection<T> _dbCollection;
    private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {

        _dbCollection = database.GetCollection<T>(collectionName);
    }


    public async Task<IReadOnlyCollection<T>> GetAllAsync()
    {
        return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
    }
    public async Task<IReadOnlyCollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbCollection.Find(filter).ToListAsync();

    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq("Id", id);
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }
    public async Task<T> CreateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbCollection.InsertOneAsync(entity);

        return entity;
    }
    public async Task<T> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        FilterDefinition<T> filter = _filterBuilder.Eq("Id", entity.Id);

        await _dbCollection.ReplaceOneAsync(filter, entity);

        return entity;
    }
    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq("Id", id);
        await _dbCollection.DeleteOneAsync(filter);
    }



}