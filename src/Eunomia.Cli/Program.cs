using Eunomia.Application.Services;
using Eunomia.Infrastructure.Persistence;
using Eunomia.Infrastructure.Repositories;

Console.WriteLine("=====================================");
Console.WriteLine("Eunomia (offline inventory) - CLI MVP");
Console.WriteLine("=====================================");

var dbPath = Path.Combine(AppContext.BaseDirectory, "data", "eunomia.db.json");
var store = new JsonStore<EunomiaDb>(dbPath);

var items = new ItemRepository(store);
var assets = new AssetRepository(store);
var ledger = new StockLedgerRepository(store);

var inventory = new InventoryService(items, assets, ledger);
var trace = new TraceabilityService(ledger);

// Seed a location + item in DB if empty (quick MVP)
var db = await store.LoadAsync(() => new EunomiaDb());
if (db.Locations.Count == 0)
{
    db.Locations.Add(new Eunomia.Domain.Entities.Location("WH-A1", "Main Warehouse"));
    await store.SaveAsync(db);
}

if (db.Items.Count == 0)
{
    await inventory.RegisterItemAsync("LAPTOP-STD", "Standard Laptop", trackByAsset: true);
}

db = await store.LoadAsync(() => new EunomiaDb());
var locId = db.Locations[0].Id;
var itemId = db.Items[0].Id;

Console.WriteLine($"DB: {dbPath}");
Console.WriteLine($"Location: {db.Locations[0].Code} ({db.Locations[0].Title})");
Console.WriteLine($"Item: {db.Items[0].Sku} ({db.Items[0].Name})");

Console.WriteLine();
Console.Write("Enter new asset tag (example: EU-0001), or press Enter to skip: ");
var tag = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(tag))
{
    var r = await inventory.RegisterAssetAsync(tag, itemId, locId);
    Console.WriteLine(r.Ok ? "Asset registered." : $"Error: {r.Error}");
}

Console.WriteLine();
Console.WriteLine("Latest traceability entries:");
var latest = await trace.LatestAsync(10);
foreach (var e in latest)
{
    Console.WriteLine($"- {e.WhenUtc:u} | {e.Action} | item={e.ItemId} | qty={e.Quantity} | note={e.Note}");
}

Console.WriteLine();
Console.WriteLine("Done.");
