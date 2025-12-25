using Eunomia.Domain.Entities;

namespace Eunomia.Infrastructure.Persistence;

public sealed class EunomiaDb
{
    public List<Item> Items { get; set; } = new();
    public List<Location> Locations { get; set; } = new();
    public List<Asset> Assets { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public List<StockLedgerEntry> Ledger { get; set; } = new();
}
