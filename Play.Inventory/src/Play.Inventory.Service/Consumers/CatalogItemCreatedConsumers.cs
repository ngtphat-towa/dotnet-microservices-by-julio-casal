using MassTransit;
using Play.Catalog.Service.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemCreatedConsumers : IConsumer<CatalogItemCreated>
{
    private readonly IRepository<CatalogItem> _catalogRepository;

    public CatalogItemCreatedConsumers(IRepository<CatalogItem> catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var message = context.Message;
        var item = await _catalogRepository.GetByIdAsync(message.ItemId);

        if (item != null)
        {
            return;
        }

        item = new CatalogItem
        {
            Id = message.ItemId,
            Name = message.Name,
            Description = message.Description,
        };
        await _catalogRepository.CreateAsync(item);
    }
}