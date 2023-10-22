using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Play.Common.Repositories;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _itemsRepository;
    private readonly ILogger<ItemsController> _logger;
    private readonly CatalogClient _catalogClient;

    public ItemsController(ILogger<ItemsController> logger,
                           IRepository<InventoryItem> itemsRepository,
                           CatalogClient catalogClient)
    {
        _logger = logger;
        _itemsRepository = itemsRepository;
        _catalogClient = catalogClient;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
    {
        // TODO: add identity
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }
        var catalogItems = await _catalogClient.GetCatalogItemsAsync();
        var inventoryItemEntities = await _itemsRepository.GetAllByFilterAsync(item => item.UserId == userId);

        var itemsRepositoryDTOs = inventoryItemEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItems?.Single(item => item.Id == inventoryItem.CatalogItemId);
            return inventoryItem.AsDTO(catalogItem!.Name, catalogItem.Description);
        });

        return Ok(itemsRepositoryDTOs);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemDTO>> PostAsync(GrantItemsDTO grantItemsDTO)
    {
        var inventoryItem = await _itemsRepository.GetByFilterAsync(item => item.UserId == grantItemsDTO.UserId
            && item.CatalogItemId == grantItemsDTO.CatalogItemId);
        InventoryItem updatedItem;
        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = grantItemsDTO.CatalogItemId,
                UserId = grantItemsDTO.UserId,
                Quantity = grantItemsDTO.Quantity,
                AcquireDate = DateTimeOffset.Now
            };
            updatedItem = await _itemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDTO.Quantity;
            updatedItem = await _itemsRepository.UpdateAsync(inventoryItem);
        }
        return Ok(updatedItem.AsDTO(String.Empty, String.Empty));

    }
}
