using Eunomia.Application.Services;
using Eunomia.Infrastructure.Persistence;
using Eunomia.Infrastructure.Repositories;
using Xunit;

public class InventoryFlowTests
{
    [Fact]
    public async Task Can_register_item_and_write_ledger()
    {
        var path = Path.Combine(Path.GetTempPath(), $"eunomia_test_{Guid.NewGuid()}.json");
        var store = new JsonStore<EunomiaDb>(path);

        var items = new ItemRepository(store);
        var assets = new AssetRepository(store);
        var ledger = new StockLedgerRepository(store);

        var inventory = new InventoryService(items, assets, ledger);

        var r = await inventory.RegisterItemAsync("TEST-1", "Test Item", false);
        Assert.True(r.Ok);

        var all = await items.GetAllAsync();
        Assert.Single(all);
    }
}
