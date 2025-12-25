using Eunomia.Domain.Common;

namespace Eunomia.Domain.Entities;

public sealed class Location : Entity
{
    public string Code { get; private set; }   // e.g., WH-A1, LAB-01
    public string Title { get; private set; }  // human name

    public Location(string code, string title)
    {
        Code = (code ?? "").Trim().ToUpperInvariant();
        Title = (title ?? "").Trim();

        if (Code.Length < 2) throw new ArgumentException("Location code is too short.");
        if (Title.Length < 2) throw new ArgumentException("Location title is too short.");
    }
}
