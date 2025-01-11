using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Sorting;
using CsvParser.Contracts.CSVs;
using CsvParser.Web.Models;

namespace CsvParser.Web.Interfaces;

public interface ICsvService
{
    Task<OperationResult<CsvPagedResult<CsvViewModel>>> GetAllAsync(string? name, string? orderBy, string? sortDirection, int? page, int? pageSize);
    Task<OperationResult<CsvViewModel>> GetByIdAsync(Guid id);
    Task<OperationResult<List<CreateCSVRequest>>> SaveRecordsAsync(List<CreateCSVRequest> records);
    Task<OperationResult<CsvViewModel>> UpdateAsync(CsvViewModel model);
    Task<OperationResult<bool>> DeleteAsync(Guid id);
}
