using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public interface IItemsRepository
{
    Task<IReadOnlyCollection<Item>> GetAll();
    Task<Item> Get(Guid id);
    Task<Item> Create(Item entity);
    Task<Item> Update(Item entity);
    Task Remove(Guid id);
}