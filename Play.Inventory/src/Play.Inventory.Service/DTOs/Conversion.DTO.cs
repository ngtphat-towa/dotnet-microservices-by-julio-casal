using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.DTOs;

public static class Extension
{
    public static InventoryItemDTO AsDTO(this InventoryItem item, string name, string description)
    {
        return new InventoryItemDTO
        (
            CatalogItemId: item.CatalogItemId,
            Quantity: item.Quantity,
            AcquireDate: item.AcquireDate,
            Name: name,
            Description: description
      );
    }
}