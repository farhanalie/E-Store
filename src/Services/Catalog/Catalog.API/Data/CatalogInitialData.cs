using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
        {
            return;
        }

        session.Store(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts()
    {
        return new List<Product>
        {
            new()
            {
                Id = new ProductId(1),
                Name = "IPhone X",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-1.png",
                Price = 950.00M,
                Category = new List<string> { "Smart Phone" }
            },
            new()
            {
                Id = new ProductId(2),
                Name = "Samsung 10",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-2.png",
                Price = 840.00M,
                Category = new List<string> { "Smart Phone" }
            },
            new()
            {
                Id = new ProductId(3),
                Name = "Huawei Plus",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-3.png",
                Price = 650.00M,
                Category = new List<string> { "White Appliances" }
            },
            new()
            {
                Id = new ProductId(4),
                Name = "Xiaomi Mi 9",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-4.png",
                Price = 470.00M,
                Category = new List<string> { "White Appliances" }
            },
            new()
            {
                Id = new ProductId(5),
                Name = "HTC U11+ Plus",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-5.png",
                Price = 380.00M,
                Category = new List<string> { "Smart Phone" }
            },
            new()
            {
                Id = new ProductId(6),
                Name = "LG G7 ThinQ",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Category = new List<string> { "Home Kitchen" }
            },
            new()
            {
                Id = new ProductId(7),
                Name = "Panasonic Lumix",
                Description =
                    "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Category = new List<string> { "Camera" }
            }
        };
    }
}
