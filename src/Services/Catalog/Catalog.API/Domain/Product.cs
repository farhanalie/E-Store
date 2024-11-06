namespace Catalog.API.Domain;

public class Product
{
    public ProductId Id { get; set; } = ProductId.From(0);
    public string Name { get; set; } = default!;
    public List<string> Category { get; set; } = [];
    public string Description { get; set; } = default!;
    public string ImageFile { get; set; } = default!;
    public decimal Price { get; set; }
}
