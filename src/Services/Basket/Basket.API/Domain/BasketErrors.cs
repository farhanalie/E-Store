namespace Basket.API.Domain;

public static class Errors
{
    public static class Basket
    {
        public static Error NotFound()
        {
            return Error.NotFound("Basket.NotFound", "Basket was not found.");
        }
    }
}