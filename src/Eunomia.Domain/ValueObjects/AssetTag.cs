using System.Text.RegularExpressions;

namespace Eunomia.Domain.ValueObjects;

public readonly record struct AssetTag(string Value)
{
    private static readonly Regex Format = new(@"^[A-Z0-9\-]{4,32}$", RegexOptions.Compiled);

    public static AssetTag Create(string raw)
    {
        raw = (raw ?? "").Trim().ToUpperInvariant();
        if (!Format.IsMatch(raw))
            throw new ArgumentException("AssetTag must be 4-32 chars, A-Z/0-9/dash only.", nameof(raw));

        return new AssetTag(raw);
    }

    public override string ToString() => Value;
}
