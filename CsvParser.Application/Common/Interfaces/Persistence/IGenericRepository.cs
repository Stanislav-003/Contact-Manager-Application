namespace CsvParser.Application.Common.Interfaces.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(Guid? id);

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task<bool> ExistsAsync(Guid entityId);

    Task DeleteAsync(Guid entityId);
}
