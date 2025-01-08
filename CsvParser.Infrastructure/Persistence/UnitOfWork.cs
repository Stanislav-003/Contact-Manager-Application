using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Infrastructure.Persistence.Repositories;

namespace CsvParser.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly CSVParserDbContext _context;
    public ICSVRepository Csvs { get; }

    public UnitOfWork(CSVParserDbContext context)
    {
        _context = context;
        Csvs = new CSVRepository(context);
    }

    public async Task CompleteAsync()
    { 
        await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
