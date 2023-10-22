using MassTransit;
using Play.Catalog.Service.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemUpdatedConsumers : IConsumer<CatalogItemUpdated>
{
    private readonly IRepository<CatalogItem> _catalogRepository;

    public CatalogItemUpdatedConsumers(IRepository<CatalogItem> catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
    {
        var message = context.Message;
        var item = await _catalogRepository.GetByIdAsync(message.ItemId);

        if (item == null)
        {
            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description,
            };
            await _catalogRepository.CreateAsync(item);

        }
        {
            item.Name = message.Name;
            item.Description = message.Description;

            await _catalogRepository.UpdateAsync(item);
        }

    }
}