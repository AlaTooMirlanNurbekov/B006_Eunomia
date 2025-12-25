using Eunomia.Domain.Common;
using Eunomia.Domain.Entities;
using Eunomia.Domain.ValueObjects;
using Eunomia.Infrastructure.Repositories;

namespace Eunomia.Application.Services;

public sealed class InventoryService
{
    private readonly ItemRepository _items;
    private readonly AssetRepository _assets;
    private readonly StockLedgerRepository _ledger;

    public InventoryService(ItemRepository items, AssetRepository assets, StockLedgerRepository ledger)
    {
        _items = items;
        _assets = assets;
        _ledger = ledger;
    }

    public async Task<Result> RegisterItemAsync(string sku, string name, bool trackByAsset)
    {
        var existing = await _items.FindBySkuAsync(sku);
        if (existing != null) return Result.Fail($"Item already exists: {sku}");

        await _items.AddAsync(new Item(sku, name, trackByAsset));
        return Result.Success();
    }

    public async Task<Result> RegisterAssetAsync(string assetTag, Guid itemId, Guid locationId)
    {
        var existing = await _assets.FindByTagAsync(assetTag);
        if (existing != null) return Result.Fail($"Asset tag already exists: {assetTag}");

        var tag = AssetTag.Create(assetTag);
        await _assets.AddAsync(new Asset(tag, itemId, locationId));

        await _ledger.AddAsync(new StockLedgerEntry
        {
            ItemId = itemId,
            AssetId = null, // could be filled once you link asset ID after save, keep MVP simple
            Action = StockAction.Receive,
            FromLocationId = locationId,
            ToLocationId = locationId,
            Quantity = 1,
            Note = $"Asset registered: {tag}"
        });

        return Result.Success();
    }

    public async Task<Result> MoveAssetAsync(string assetTag, Guid toLocationId, string? note = null)
    {
        var asset = await _assets.FindByTagAsync(assetTag);
        if (asset == null) return Result.Fail($"Asset not found: {assetTag}");

        var from = asset.CurrentLocationId;
        asset.MoveTo(toLocationId);
        await _assets.UpdateAsync(asset);

        await _ledger.AddAsync(new StockLedgerEntry
        {
            ItemId = asset.ItemId,
            AssetId = asset.Id,
            Action = StockAction.Transfer,
            FromLocationId = from,
            ToLocationId = toLocationId,
            Quantity = 1,
            Note = note ?? $"Asset moved: {asset.Tag}"
        });

        return Result.Success();
    }
}
