using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories;

public interface IItemsRepository
{
    Task<IReadOnlyCollection<Item>> GetAllAsync();
    Task<Item> GetByIdAsync(Guid id);
    Task<Item> CreateAsync(Item entity);
    Task<Item> UpdateAsync(Item entity);
    Task RemoveAsync(Guid id);
}