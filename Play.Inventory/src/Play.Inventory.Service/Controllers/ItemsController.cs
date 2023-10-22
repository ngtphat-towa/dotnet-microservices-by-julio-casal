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
    private readonly IRepository<InventoryItem> _inventoryItemsRepository;
    private readonly IRepository<CatalogItem> _catalogItemsRepository;
    private readonly ILogger<ItemsController> _logger;


    public ItemsController(ILogger<ItemsController> logger,
                           IRepository<InventoryItem> inventoryItemsRepository,
                           IRepository<CatalogItem> catalogItemsRepository)
    {
        _logger = logger;
        _inventoryItemsRepository = inventoryItemsRepository;
        _catalogItemsRepository = catalogItemsRepository;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
    {
        // TODO: add identity
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }
        // Get all item belong to the user that have userId
        var inventoryItemEntities = await _inventoryItemsRepository.GetAllByFilterAsync(item => item.UserId == userId);
        // Filter the id in the list 
        var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
        // Take all catalog that the list Ids contains the id
        var catalogItems = await _catalogItemsRepository.GetAllByFilterAsync(item => itemIds.Contains(item.Id));

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
        var inventoryItem = await _inventoryItemsRepository.GetByFilterAsync(item => item.UserId == grantItemsDTO.UserId
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
            updatedItem = await _inventoryItemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDTO.Quantity;
            updatedItem = await _inventoryItemsRepository.UpdateAsync(inventoryItem);
        }
        return Ok(updatedItem.AsDTO(String.Empty, String.Empty));

    }
}
