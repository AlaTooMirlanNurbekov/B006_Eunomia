using Eunomia.Domain.Entities;
using Eunomia.Infrastructure.Persistence;

namespace Eunomia.Infrastructure.Repositories;

public sealed class StockLedgerRepository
{
    private readonly JsonStore<EunomiaDb> _store;
    public StockLedgerRepository(JsonStore<EunomiaDb> store) => _store = store;

    public async Task AddAsync(StockLedgerEntry entry)
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        db.Ledger.Add(entry);
        await _store.SaveAsync(db);
    }

    public async Task<List<StockLedgerEntry>> GetLatestAsync(int take = 20)
    {
        var db = await _store.LoadAsync(() => new EunomiaDb());
        return db.Ledger
            .OrderByDescending(x => x.WhenUtc)
            .Take(take)
            .ToList();
    }
}
