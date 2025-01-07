using CsvParser.Domain.Models;

namespace CsvParser.Application.Common.Interfaces.Persistence;

public interface ICSVRepository : IGenericRepository<CsvParser.Domain.Models.CSV>
{
    Task<List<CsvParser.Domain.Models.CSV>> GetAllAsync();

    Task<CsvParser.Domain.Models.CSV?> GetByIdAsync(Guid? id);

    Task AddAsync(CsvParser.Domain.Models.CSV csv);

    Task UpdateAsync(CsvParser.Domain.Models.CSV csv);

    Task<bool> ExistsAsync(Guid csvId);

    Task DeleteAsync(Guid id);
}
