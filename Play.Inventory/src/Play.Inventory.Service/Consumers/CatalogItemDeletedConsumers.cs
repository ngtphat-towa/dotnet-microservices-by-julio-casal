using MassTransit;
using Play.Catalog.Service.Contracts;
using Play.Common.Repositories;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers;

public class CatalogItemDeletedConsumers : IConsumer<CatalogItemDeleted>
{
    private readonly IRepository<CatalogItem> _catalogRepository;

    public CatalogItemDeletedConsumers(IRepository<CatalogItem> catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
    {
        var message = context.Message;
        var item = await _catalogRepository.GetByIdAsync(message.ItemId);

        if (item == null)
        {
            return;
        }

        await _catalogRepository.RemoveAsync(message.ItemId);
    }
}