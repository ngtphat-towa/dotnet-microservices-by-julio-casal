using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Play.Common.Repositories;
using Play.Inventory.Service.DTOs;
using Play.Inventory.Service.Entites;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _itemsRepository;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(ILogger<ItemsController> logger, IRepository<InventoryItem> itemsRepository)
    {
        _logger = logger;
        _itemsRepository = itemsRepository;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetAsync(Guid userId)
    {
        // TODO: add identity
        if (userId == Guid.Empty)
        {
            return BadRequest();
        }
        var items = (await _itemsRepository.GetAllByFilterAsync(item => item.UserId == userId)).Select(item => item.AsDTO());

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemDTO>> PostAsync(GrantItemsDTO grantItemsDTO)
    {
        var inventoryItem = await _itemsRepository.GetByFilterAsync(item => item.UserId == grantItemsDTO.UserId
            && item.CatalogItemId == grantItemsDTO.CatalogItemId);
        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = grantItemsDTO.CatalogItemId,
                UserId = grantItemsDTO.UserId,
                Quantity = grantItemsDTO.Quantity,
                AcquireDate = DateTimeOffset.Now
            };
            inventoryItem = await _itemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDTO.Quantity;
            inventoryItem = await _itemsRepository.UpdateAsync(inventoryItem);
        }
        return Ok(inventoryItem.AsDTO());

    }
}
