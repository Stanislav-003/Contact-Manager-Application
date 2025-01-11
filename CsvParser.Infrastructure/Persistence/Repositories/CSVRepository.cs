using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Application.Common.Sorting;
using CsvParser.Contracts.CSVs;
using CsvParser.Domain.Models;
using CsvParser.Infrastructure.Extensions;
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

    //public override Task<List<CSV>> GetAllAsync()
    //{
    //    return base.GetAllAsync();
    //}

    public async Task<CsvPagedResult<CSV>> GetAllAsync(
        CsvFilter csvFilter, 
        SortParams sortParams, 
        PageParams pageParams)
    {
        //return _context.CSVs.Filter(csvFilter).ToListAsync();
        
        var result = await _context
            .CSVs
            .Filter(csvFilter)
            .Sort(sortParams)
            .ToPageAsync(pageParams);
        
        return result;
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
