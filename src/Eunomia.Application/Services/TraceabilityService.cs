using Eunomia.Domain.Entities;
using Eunomia.Infrastructure.Repositories;

namespace Eunomia.Application.Services;

public sealed class TraceabilityService
{
    private readonly StockLedgerRepository _ledger;

    public TraceabilityService(StockLedgerRepository ledger) => _ledger = ledger;

    public async Task<List<StockLedgerEntry>> LatestAsync(int take = 20)
        => await _ledger.GetLatestAsync(take);
}
