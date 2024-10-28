namespace Catalog.API.Domain;

public static class Errors
{
    public static class Product
    {
        public static Error NotFound(ProductId id)
        {
            return Error.NotFound(
                "Product.NotFound",
                $"Product with id {id} was not found."
            );
        }

        public static Error IdMismatch(ProductId id)
        {
            return Error.Validation(
                "Product.IdMismatch",
                $"Product id {id} does not match the id in the request."
            );
        }
    }
}