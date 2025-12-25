namespace Eunomia.Domain.Common;

public sealed class Result
{
    public bool Ok { get; }
    public string? Error { get; }

    private Result(bool ok, string? error)
    {
        Ok = ok;
        Error = error;
    }

    public static Result Success() => new(true, null);
    public static Result Fail(string error) => new(false, error);
}
