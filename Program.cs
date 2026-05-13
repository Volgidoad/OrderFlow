using OrderFlow.Console.Data;
using OrderFlow.Console.Models;
using OrderFlow.Console.Services;

var orders = SampleData.Orders;

Console.WriteLine("=== TASK 2 - VALIDATION ===");

var validator = new OrderValidator();

var validOrder = orders[0];

var invalidOrder = new Order
{
    Id = 9999,
    Customer = SampleData.Customers[0],
    OrderDate = DateTime.Now.AddDays(2),
    Status = OrderStatus.Cancelled,
    Items =
    [
        new()
        {
            Product = SampleData.Products[0],
            ProductId = 1,
            Quantity = 0
        }
    ]
};

ShowValidation(validOrder, validator);
ShowValidation(invalidOrder, validator);

Console.WriteLine("\n=== TASK 3 - ACTION / FUNC / PREDICATE ===");

var processor = new OrderProcessor(orders);

// Predicate examples
Predicate<Order> completedOrders = o => o.Status == OrderStatus.Completed;
Predicate<Order> vipOrders = o => o.Customer.IsVip;
Predicate<Order> expensiveOrders = o => o.TotalAmount > 1000m;

Console.WriteLine("\nCompleted orders:");
foreach (var order in processor.FilterOrders(completedOrders))
    Console.WriteLine($"Order #{order.Id} - {order.TotalAmount:C}");

Console.WriteLine("\nVIP orders:");
foreach (var order in processor.FilterOrders(vipOrders))
    Console.WriteLine($"Order #{order.Id} - {order.Customer.FullName}");

// Action examples
Action<Order> printOrder = o =>
    Console.WriteLine($"PRINT -> Order #{o.Id} | {o.Customer.FullName} | {o.TotalAmount:C}");

Action<Order> markProcessing = o => o.Status = OrderStatus.Processing;

processor.ProcessOrders(printOrder);
processor.ProcessOrders(markProcessing);

// Func examples
var anonymousProjection = processor.ProjectOrders(o => new
{
    o.Id,
    Customer = o.Customer.FullName,
    o.TotalAmount
});

Console.WriteLine("\nAnonymous projection:");
foreach (var item in anonymousProjection)
    Console.WriteLine($"{item.Id} | {item.Customer} | {item.TotalAmount:C}");

// Aggregation examples
var totalRevenue = processor.Aggregate(o => o.Sum(x => x.TotalAmount));
var averageRevenue = processor.Aggregate(o => o.Average(x => x.TotalAmount));
var maxRevenue = processor.Aggregate(o => o.Max(x => x.TotalAmount));

Console.WriteLine($"\nTotal revenue: {totalRevenue:C}");
Console.WriteLine($"Average revenue: {averageRevenue:C}");
Console.WriteLine($"Max revenue: {maxRevenue:C}");

// Combined flow
Console.WriteLine("\nCombined flow:");
var topOrders = processor
    .FilterOrders(expensiveOrders)
    .OrderByDescending(o => o.TotalAmount)
    .Take(3);

foreach (var order in topOrders)
    printOrder(order);

Console.WriteLine("\n=== TASK 4 - LINQ ===");

// 1. Method syntax - filtering
Console.WriteLine("\n1. Expensive orders (method syntax)");
var expensive = orders
    .Where(o => o.TotalAmount > 1000m)
    .OrderByDescending(o => o.TotalAmount);

foreach (var order in expensive)
    Console.WriteLine($"Order #{order.Id} - {order.TotalAmount:C}");

// 2. Query syntax - grouping by customer city
Console.WriteLine("\n2. Orders grouped by city (query syntax)");
var groupedByCity =
    from o in orders
    group o by o.Customer.City into cityGroup
    select new
    {
        City = cityGroup.Key,
        Count = cityGroup.Count(),
        Total = cityGroup.Sum(x => x.TotalAmount)
    };

foreach (var group in groupedByCity)
    Console.WriteLine($"{group.City} | Orders: {group.Count} | Total: {group.Total:C}");

// 3. Join orders with customers
Console.WriteLine("\n3. Join orders with customers");
var joined =
    from o in orders
    join c in SampleData.Customers on o.Customer.Id equals c.Id
    select new
    {
        o.Id,
        Customer = c.FullName,
        c.City,
        o.TotalAmount
    };

foreach (var item in joined)
    Console.WriteLine($"{item.Id} | {item.Customer} | {item.City} | {item.TotalAmount:C}");

// 4. SelectMany flattening
Console.WriteLine("\n4. SelectMany flattening");
var flattened = orders
    .SelectMany(o => o.Items,
        (order, item) => new
        {
            order.Id,
            Product = item.Product.Name,
            item.Quantity
        });

foreach (var item in flattened)
    Console.WriteLine($"Order #{item.Id} | {item.Product} | Qty: {item.Quantity}");

// 5. GroupBy aggregation
Console.WriteLine("\n5. Top customers by amount");
var topCustomers = orders
    .GroupBy(o => o.Customer.FullName)
    .Select(g => new
    {
        Customer = g.Key,
        Total = g.Sum(x => x.TotalAmount)
    })
    .OrderByDescending(x => x.Total);

foreach (var customer in topCustomers)
    Console.WriteLine($"{customer.Customer} | {customer.Total:C}");

// 6. GroupJoin (left join pattern)
Console.WriteLine("\n6. GroupJoin");
var customerOrders =
    from c in SampleData.Customers
    join o in orders on c.Id equals o.Customer.Id into customerGroup
    select new
    {
        Customer = c.FullName,
        Orders = customerGroup.Count(),
        Total = customerGroup.Sum(x => x.TotalAmount)
    };

foreach (var item in customerOrders)
    Console.WriteLine($"{item.Customer} | Orders: {item.Orders} | Total: {item.Total:C}");

// 7. Mixed syntax
Console.WriteLine("\n7. Mixed syntax");
var mixed =
    (from o in orders
     where o.Customer.IsVip
     select o)
    .GroupBy(o => o.Customer.FullName)
    .Select(g => new
    {
        Customer = g.Key,
        FavoriteCategory = g
            .SelectMany(o => o.Items)
            .GroupBy(i => i.Product.Category)
            .OrderByDescending(x => x.Count())
            .First().Key
    });

foreach (var item in mixed)
    Console.WriteLine($"{item.Customer} | Favorite category: {item.FavoriteCategory}");

static void ShowValidation(Order order, OrderValidator validator)
{
    var errors = validator.ValidateAll(order);

    Console.WriteLine($"\nOrder #{order.Id}");

    if (errors.Count == 0)
    {
        Console.WriteLine("Validation passed.");
    }
    else
    {
        foreach (var error in errors)
            Console.WriteLine($"ERROR: {error}");
    }
}