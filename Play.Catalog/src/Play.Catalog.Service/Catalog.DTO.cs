namespace Play.Catalog.Service.DTOs
{
    public record ItemDTO(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedDate);
    public record CreateDTO(string Name, string Description, decimal Price);
    public record UpdateItemDTO(string Name, string Description, decimal Price);

}