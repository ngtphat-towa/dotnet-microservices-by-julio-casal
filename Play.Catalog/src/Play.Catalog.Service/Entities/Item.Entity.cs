namespace Play.Catalog.Service.Entities;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Price { get; set; } = "";
    public DateTimeOffset CreatedDate { get; set; }

}