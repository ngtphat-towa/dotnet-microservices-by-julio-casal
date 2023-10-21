using System.Collections.ObjectModel;
using Play.Inventory.Service.Controllers;
using Play.Inventory.Service.DTOs;

namespace Play.Inventory.Service.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpClient;

    public CatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<IReadOnlyCollection<CatalogItemDTO>> GetCatalogItemsAsync()
    {
        var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDTO>>("/items");
        return items ?? Array.Empty<CatalogItemDTO>();
    }
}