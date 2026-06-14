using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public class OrderProcessor
{
    private readonly List<Order> _orders;

    public OrderProcessor(List<Order> orders)
    {
        _orders = orders;
    }

    public IEnumerable<Order> FilterOrders(Predicate<Order> predicate)
        => _orders.Where(o => predicate(o));

    public void ProcessOrders(Action<Order> action)
    {
        foreach (var order in _orders)
            action(order);
    }

    public IEnumerable<T> ProjectOrders<T>(Func<Order, T> selector)
        => _orders.Select(selector);

    public decimal Aggregate(Func<IEnumerable<Order>, decimal> aggregator)
        => aggregator(_orders);
}