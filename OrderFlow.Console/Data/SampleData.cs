using OrderFlow.Console.Models;

namespace OrderFlow.Console.Data;

public static class SampleData
{
    public static List<Product> Products { get; } =
    [
        new() { Id = 1, Name = "Shure SM7B", Category = "Audio", Price = 1899m },
        new() { Id = 2, Name = "Kabel XLR", Category = "Accessories", Price = 49.99m },
        new() { Id = 3, Name = "Focusrite Scarlett", Category = "Audio", Price = 899m },
        new() { Id = 4, Name = "Pop Filter", Category = "Accessories", Price = 39.99m },
        new() { Id = 5, Name = "Logitech MX Master 3", Category = "Office", Price = 499m }
    ];

    public static List<Customer> Customers { get; } =
    [
        new() { Id = 1, FullName = "Jan Kowalski", City = "Warsaw", IsVip = true },
        new() { Id = 2, FullName = "Anna Nowak", City = "Krakow", IsVip = false },
        new() { Id = 3, FullName = "Piotr Zielinski", City = "Warsaw", IsVip = false },
        new() { Id = 4, FullName = "Maria Wisniewska", City = "Gdansk", IsVip = true }
    ];

    public static List<Order> Orders { get; } =
    [
        new()
        {
            Id = 1001,
            Customer = Customers[0],
            OrderDate = DateTime.Today.AddDays(-3),
            Status = OrderStatus.Completed,
            Items =
            [
                new() { Product = Products[0], ProductId = 1, Quantity = 1 },
                new() { Product = Products[1], ProductId = 2, Quantity = 2 }
            ]
        },
        new()
        {
            Id = 1002,
            Customer = Customers[1],
            OrderDate = DateTime.Today.AddDays(-2),
            Status = OrderStatus.Processing,
            Items =
            [
                new() { Product = Products[2], ProductId = 3, Quantity = 1 }
            ]
        },
        new()
        {
            Id = 1003,
            Customer = Customers[2],
            OrderDate = DateTime.Today.AddDays(-1),
            Status = OrderStatus.New,
            Items =
            [
                new() { Product = Products[4], ProductId = 5, Quantity = 1 },
                new() { Product = Products[3], ProductId = 4, Quantity = 3 }
            ]
        },
        new()
        {
            Id = 1004,
            Customer = Customers[3],
            OrderDate = DateTime.Today,
            Status = OrderStatus.Validated,
            Items =
            [
                new() { Product = Products[0], ProductId = 1, Quantity = 2 }
            ]
        },
        new()
        {
            Id = 1005,
            Customer = Customers[0],
            OrderDate = DateTime.Today.AddDays(-7),
            Status = OrderStatus.Cancelled,
            Items =
            [
                new() { Product = Products[1], ProductId = 2, Quantity = 5 }
            ]
        },
        new()
        {
            Id = 1006,
            Customer = Customers[1],
            OrderDate = DateTime.Today.AddDays(-10),
            Status = OrderStatus.Completed,
            Items =
            [
                new() { Product = Products[2], ProductId = 3, Quantity = 2 },
                new() { Product = Products[4], ProductId = 5, Quantity = 1 }
            ]
        }
    ];
}