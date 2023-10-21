namespace Play.Inventory.Service.DTOs;
public record GrantItemsDTO(Guid UserId, Guid CatalogItemId, int Quantity);
public record InventoryItemDTO(Guid CatalogItemId, int Quantity, DateTimeOffset AcquireDate);