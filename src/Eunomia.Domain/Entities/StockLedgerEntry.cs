using Eunomia.Domain.Common;

namespace Eunomia.Domain.Entities;

public enum StockAction
{
    Receive,
    Consume,
    Transfer
}

public sealed class StockLedgerEntry : Entity
{
    public DateTime WhenUtc { get; init; } = DateTime.UtcNow;

    public Guid ItemId { get; init; }
    public Guid? AssetId { get; init; } // optional if not asset-tracked

    public StockAction Action { get; init; }

    public Guid FromLocationId { get; init; }
    public Guid ToLocationId { get; init; }

    public int Quantity { get; init; } // for non-asset items
    public string? Note { get; init; }
}
