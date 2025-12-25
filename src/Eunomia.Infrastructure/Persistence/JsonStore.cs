using System.Text.Json;

namespace Eunomia.Infrastructure.Persistence;

public sealed class JsonStore<T> where T : class
{
    private readonly string _path;
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public JsonStore(string path)
    {
        _path = path;
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
    }

    public async Task<T> LoadAsync(Func<T> emptyFactory)
    {
        if (!File.Exists(_path)) return emptyFactory();

        var json = await File.ReadAllTextAsync(_path);
        if (string.IsNullOrWhiteSpace(json)) return emptyFactory();

        var data = JsonSerializer.Deserialize<T>(json, Options);
        return data ?? emptyFactory();
    }

    public async Task SaveAsync(T data)
    {
        var json = JsonSerializer.Serialize(data, Options);
        await File.WriteAllTextAsync(_path, json);
    }
}
