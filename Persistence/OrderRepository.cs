using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Console.Models;

namespace OrderFlow.Console.Persistence;

public class OrderRepository
{
    private readonly OrderFlowContext _context;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public OrderRepository(OrderFlowContext context)
    {
        _context = context;
    }

    public async Task ImportFromJsonToDbAsync(string path)
    {
        if (!File.Exists(path)) return;

        await using var stream = File.OpenRead(path);
        var orders = await JsonSerializer.DeserializeAsync<List<Order>>(stream);

        if (orders != null)
        {
            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
            System.Console.WriteLine($"[Repo] Imported {orders.Count} orders from JSON to Database.");
        }
    }

    public async Task ExportDbToJsonAsync(string path)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        await using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, orders, _jsonOptions);
        System.Console.WriteLine($"[Repo] Database exported to {path}");
    }
}