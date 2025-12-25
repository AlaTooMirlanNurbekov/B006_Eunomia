using Eunomia.Domain.Common;

namespace Eunomia.Domain.Entities;

public enum OrderStatus
{
    Draft,
    Submitted,
    Completed,
    Cancelled
}

public sealed class Order : Entity
{
    public string Number { get; private set; } // e.g., ORD-2025-0001
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public List<OrderLine> Lines { get; private set; } = new();

    public Order(string number)
    {
        Number = (number ?? "").Trim().ToUpperInvariant();
        if (Number.Length < 6) throw new ArgumentException("Order number is too short.");
    }

    public void AddLine(Guid itemId, int qty)
    {
        if (Status != OrderStatus.Draft) throw new InvalidOperationException("Only draft orders can be edited.");
        if (qty <= 0) throw new ArgumentException("Quantity must be > 0.");

        Lines.Add(new OrderLine(itemId, qty));
    }

    public void Submit()
    {
        if (Lines.Count == 0) throw new InvalidOperationException("Order must have at least one line.");
        Status = OrderStatus.Submitted;
    }

    public void Complete() => Status = OrderStatus.Completed;
    public void Cancel() => Status = OrderStatus.Cancelled;
}

public sealed class OrderLine
{
    public Guid ItemId { get; init; }
    public int Quantity { get; init; }

    public OrderLine(Guid itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }
}
