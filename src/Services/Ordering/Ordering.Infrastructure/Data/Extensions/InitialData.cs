namespace Ordering.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
    [
        Customer.Create(CustomerId.From(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")), "mehmet",
            "mehmet@gmail.com"),
        Customer.Create(CustomerId.From(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")), "john",
            "john@gmail.com")
    ];

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.From(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "IPhone X", 500),
            Product.Create(ProductId.From(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "Samsung 10", 400),
            Product.Create(ProductId.From(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), "Huawei Plus", 650),
            Product.Create(ProductId.From(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), "Xiaomi Mi", 450)
        };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            Address address1 = Address.From("mehmet", "ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey",
                "Istanbul",
                "38050");
            Address address2 = Address.From("john", "doe", "john@gmail.com", "Broadway No:1", "England", "Nottingham",
                "08050");

            Payment payment1 = Payment.From("mehmet", "5555555555554444", "12/28", "355", 1);
            Payment payment2 = Payment.From("john", "8885555555554444", "06/30", "222", 2);

            Order order1 = Order.Create(
                OrderId.From(Guid.NewGuid()),
                CustomerId.From(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),
                OrderName.From("ORD_1"),
                address1,
                address1,
                payment1);
            order1.Add(ProductId.From(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), 2, 500);
            order1.Add(ProductId.From(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), 1, 400);

            Order order2 = Order.Create(
                OrderId.From(Guid.NewGuid()),
                CustomerId.From(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")),
                OrderName.From("ORD_2"),
                address2,
                address2,
                payment2);
            order2.Add(ProductId.From(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), 1, 650);
            order2.Add(ProductId.From(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), 2, 450);

            return [order1, order2];
        }
    }
}
