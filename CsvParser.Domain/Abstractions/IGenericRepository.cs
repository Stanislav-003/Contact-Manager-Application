using System.Threading.Tasks;

namespace CsvParser.Domain.Abstractions;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> ExistsAsync(Guid csvId);
    Task DeleteAsync(Guid id);
}
