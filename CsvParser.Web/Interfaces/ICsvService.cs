using CsvParser.Contracts.CSVs;
using CsvParser.Web.Models;

namespace CsvParser.Web.Interfaces;

public interface ICsvService
{
    Task<OperationResult<IEnumerable<CsvViewModel>>> GetAllAsync();
    Task<OperationResult<CsvViewModel>> GetByIdAsync(Guid id);
    Task<OperationResult<List<CreateCSVRequest>>> SaveRecordsAsync(List<CreateCSVRequest> records);
    Task<OperationResult<CsvViewModel>> UpdateAsync(CsvViewModel model);
    Task<OperationResult<bool>> DeleteAsync(Guid id);
}
