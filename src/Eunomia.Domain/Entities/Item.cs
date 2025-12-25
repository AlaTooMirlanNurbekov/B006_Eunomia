using Eunomia.Domain.Common;

namespace Eunomia.Domain.Entities;

public sealed class Item : Entity
{
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public bool TrackByAsset { get; private set; } // true = each unit may have AssetTag(s)

    public Item(string sku, string name, bool trackByAsset)
    {
        Sku = (sku ?? "").Trim();
        Name = (name ?? "").Trim();
        TrackByAsset = trackByAsset;

        if (Sku.Length < 2) throw new ArgumentException("SKU is too short.");
        if (Name.Length < 2) throw new ArgumentException("Name is too short.");
    }
}
