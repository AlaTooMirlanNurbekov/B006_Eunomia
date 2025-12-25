using Eunomia.Domain.Common;
using Eunomia.Domain.ValueObjects;

namespace Eunomia.Domain.Entities;

public sealed class Asset : Entity
{
    public AssetTag Tag { get; private set; }
    public Guid ItemId { get; private set; }       // which Item this asset belongs to
    public Guid CurrentLocationId { get; private set; }
    public string Status { get; private set; } = "Active"; // Active, Repair, Retired

    public Asset(AssetTag tag, Guid itemId, Guid locationId)
    {
        Tag = tag;
        ItemId = itemId;
        CurrentLocationId = locationId;
    }

    public void MoveTo(Guid newLocationId) => CurrentLocationId = newLocationId;

    public void SetStatus(string status)
    {
        status = (status ?? "").Trim();
        if (status.Length < 3) throw new ArgumentException("Invalid status.");
        Status = status;
    }
}
