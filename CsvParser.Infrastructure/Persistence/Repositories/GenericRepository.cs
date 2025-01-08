using CsvParser.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace CsvParser.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly CSVParserDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(CSVParserDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        _dbSet.Remove(entity!);
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        //return await _dbSet.FindAsync(id) != null;
        return await _dbSet.AnyAsync(entity => EF.Property<Guid>(entity, "Id") == id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }
}
