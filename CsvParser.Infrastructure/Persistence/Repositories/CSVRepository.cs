using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Domain.Models;
using CsvParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CsvParser.Infrastructure.Persistence.Repositories;

public class CSVRepository : GenericRepository<CSV>, ICSVRepository
{
    public CSVRepository(CSVParserDbContext context) 
        : base(context)
    {
    }

    public override Task AddAsync(CSV csv)
    {
        return base.AddAsync(csv);
    }

    public override Task DeleteAsync(Guid id)
    {
        return base.DeleteAsync(id);
    }

    public override Task<bool> ExistsAsync(Guid csvId)
    {
        return base.ExistsAsync(csvId);
    }

    public override Task<List<CSV>> GetAllAsync()
    {
        return base.GetAllAsync();
    }

    public override Task<CSV?> GetByIdAsync(Guid id)
    {
        return base.GetByIdAsync(id);
    }

    public override Task UpdateAsync(CSV csv)
    {
        return base.UpdateAsync(csv);
    }
}
