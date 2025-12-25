using Eunomia.Domain.Entities;
using Eunomia.Infrastructure.Persistence;

namespace Eunomia.Infrastructure.Repositories;

public sealed class AssetRepository
{
    private readonly JsonStore<EunomiaDb> _store;
    public AssetRepository(JsonStore<EunomiaDb> store) => _store = store;

    public async Task<Asset> AddAsync(Asset asset)
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        db.Assets.Add(asset);
        await _store.SaveAsync(db);
        return asset;
    }

    public async Task<Asset?> FindByTagAsync(string tagValue)
    {
        var tag = (tagValue ?? "").Trim().ToUpperInvariant();
        var db = await _store.LoadAsync(() => new EunomiaDb());
        return db.Assets.FirstOrDefault(a => a.Tag.Value == tag);
    }

    public async Task UpdateAsync(Asset asset)
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        var idx = db.Assets.FindIndex(a => a.Id == asset.Id);
        if (idx >= 0) db.Assets[idx] = asset;
        await _store.SaveAsync(db);
    }
}
