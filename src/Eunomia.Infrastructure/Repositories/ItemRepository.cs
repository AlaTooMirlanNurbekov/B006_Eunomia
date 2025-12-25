using Eunomia.Domain.Entities;
using Eunomia.Infrastructure.Persistence;

namespace Eunomia.Infrastructure.Repositories;

public sealed class ItemRepository
{
    private readonly JsonStore<EunomiaDb> _store;
    public ItemRepository(JsonStore<EunomiaDb> store) => _store = store;

    public async Task<Item> AddAsync(Item item)
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        db.Items.Add(item);
        await _store.SaveAsync(db);
        return item;
    }

    public async Task<Item?> FindBySkuAsync(string sku)
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        return db.Items.FirstOrDefault(x => x.Sku.Equals(sku.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public async Task<List<Item>> GetAllAsync()
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        return db.Items.ToList();
    }
}
