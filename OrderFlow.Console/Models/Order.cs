namespace OrderFlow.Console.Models;

public class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItem> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
}