using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Domain.Models;
using CsvParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CsvParser.Infrastructure.Persistence.Repositories;

public class CSVRepository : ICSVRepository
{
    private readonly CSVParserDbContext _context;

    public CSVRepository(CSVParserDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CSV csv)
    {
        _context.CSVs.Add(csv);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        //await _context.CSVs
        //    .Where(csv => csv.Id == id)
        //    .ExecuteDeleteAsync();

        var csv = await _context.CSVs.FirstOrDefaultAsync(csv => csv.Id == id);

        _context.CSVs.Remove(csv);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CSV>> GetAllAsync()
    {
        return await _context.CSVs.ToListAsync();
    }

    public async Task<CSV?> GetByIdAsync(Guid? id)
    {
        return await _context.CSVs.FirstOrDefaultAsync(csv => csv.Id == id);
    }

    public Task<IEnumerable<CSV>> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(Guid csvId)
    {
        return await _context.CSVs.AnyAsync(csv => csv.Id == csvId);
    }

    public async Task UpdateAsync(CSV csv)
    {
        _context.CSVs.Update(csv);

        await _context.SaveChangesAsync();
    }
}
