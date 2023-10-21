using Play.Inventory.Service.Entites;

namespace Play.Inventory.Service.DTOs;

public static class Extension
{
    public static InventoryItemDTO AsDTO(this InventoryItem item)
    {
        return new InventoryItemDTO
        (
            CatalogItemId: item.CatalogItemId,
            Quantity: item.Quantity,
            AcquireDate: item.AcquireDate
        );
    }
}