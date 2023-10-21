using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.Entities;
namespace Play.Catalog.Service.Extensions;
public static class ConversationX
{
    public static ItemDTO AsDTO(this Item item)
    {
        return new ItemDTO(Id: item.Id, Name: item.Name, Description: item.Description, Price: item.Price, CreatedDate: item.CreatedDate);
    }
} 