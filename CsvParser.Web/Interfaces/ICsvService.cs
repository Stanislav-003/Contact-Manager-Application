using CsvParser.Contracts.CSVs;
using CsvParser.Web.Models;

namespace CsvParser.Web.Interfaces;

public interface ICsvService
{
    Task<IEnumerable<CsvViewModel>> GetAllAsync();
    Task<CsvViewModel?> GetByIdAsync(Guid id);
    Task<List<(CreateCSVRequest Record, string Error)>> SaveRecordsAsync(List<CreateCSVRequest> records);
    Task<(bool Success, string? Error)> UpdateAsync(CsvViewModel model);
    Task<(bool Success, string? Error)> DeleteAsync(Guid id);
}
