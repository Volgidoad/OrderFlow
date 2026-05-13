using OrderFlow.Console.Models;

namespace OrderFlow.Console.Services;

public delegate bool ValidationRule(Order order, out string errorMessage);

public class OrderValidator
{
    private readonly List<ValidationRule> _rules = [];
    private readonly List<(Func<Order, bool> Rule, string Message)> _funcRules = [];

    public OrderValidator()
    {
        _rules.Add(HasItems);
        _rules.Add(ValidQuantity);
        _rules.Add(MaxOrderLimit);

        _funcRules.Add((o => o.OrderDate <= DateTime.Now, "Order date cannot be in the future."));
        _funcRules.Add((o => o.Status != OrderStatus.Cancelled, "Cancelled orders are not allowed."));
    }

    public List<string> ValidateAll(Order order)
    {
        List<string> errors = [];

        foreach (var rule in _rules)
        {
            if (!rule(order, out string error))
                errors.Add(error);
        }

        foreach (var (rule, message) in _funcRules)
        {
            if (!rule(order))
                errors.Add(message);
        }

        return errors;
    }

    private bool HasItems(Order order, out string errorMessage)
    {
        if (order.Items.Count == 0)
        {
            errorMessage = "Order must contain at least one item.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private bool ValidQuantity(Order order, out string errorMessage)
    {
        if (order.Items.Any(i => i.Quantity <= 0))
        {
            errorMessage = "All quantities must be greater than zero.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private bool MaxOrderLimit(Order order, out string errorMessage)
    {
        if (order.TotalAmount > 10000m)
        {
            errorMessage = "Order exceeds maximum allowed amount.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}