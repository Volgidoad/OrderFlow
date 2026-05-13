namespace OrderFlow.Console.Models;

public class OrderItem
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }

    public decimal TotalPrice => Product.Price * Quantity;
}