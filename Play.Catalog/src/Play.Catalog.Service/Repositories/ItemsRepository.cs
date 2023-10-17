using System.Data.Common;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories.Contracts;

namespace Play.Catalog.Service.Repositories;

public class ItemsRepository : IItemsRepository
{

    private const string _dbName = "Catalog";
    private const string _collectionName = "items";
    private readonly IMongoCollection<Item> _dbCollection;
    private const string _connectionString = "mongodb://localhost:27017";
    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public ItemsRepository()
    {
        var mongoClient = new MongoClient(_connectionString);
        var database = mongoClient.GetDatabase(_dbName);
        _dbCollection = database.GetCollection<Item>(_collectionName);
    }


    public async Task<IReadOnlyCollection<Item>> GetAll()
    {
        return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> Get(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq("Id", id);
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Item> Create(Item entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbCollection.InsertOneAsync(entity);

        return entity;
    }
    public async Task<Item> Update(Item entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        FilterDefinition<Item> filter = _filterBuilder.Eq("Id", entity.Id);

        await _dbCollection.ReplaceOneAsync(filter, entity);

        return entity;
    }
    public async Task Remove(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq("Id", id);
        await _dbCollection.DeleteOneAsync(filter);
    }
}