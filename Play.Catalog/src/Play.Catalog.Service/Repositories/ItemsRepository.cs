using System.Data.Common;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
namespace Play.Catalog.Service.Repositories;

public class ItemsRepository : IItemsRepository
{

    private const string _collectionName = "items";
    private readonly IMongoCollection<Item> _dbCollection;
    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public ItemsRepository(IMongoDatabase database)
    {

        _dbCollection = database.GetCollection<Item>(_collectionName);
    }


    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<Item> GetByIdAsync(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq("Id", id);
        return await _dbCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Item> CreateAsync(Item entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbCollection.InsertOneAsync(entity);

        return entity;
    }
    public async Task<Item> UpdateAsync(Item entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        FilterDefinition<Item> filter = _filterBuilder.Eq("Id", entity.Id);

        await _dbCollection.ReplaceOneAsync(filter, entity);

        return entity;
    }
    public async Task RemoveAsync(Guid id)
    {
        FilterDefinition<Item> filter = _filterBuilder.Eq("Id", id);
        await _dbCollection.DeleteOneAsync(filter);
    }
}