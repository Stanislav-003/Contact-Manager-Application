using CsvParser.Domain.Models;
using CsvParser.Domain.Abstractions;
using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Sorting;
using CsvParser.Contracts.CSVs;

namespace CsvParser.Application.Common.Interfaces.Persistence;

public interface ICSVRepository : IGenericRepository<CsvParser.Domain.Models.CSV>
{
    Task<CsvPagedResult<CsvParser.Domain.Models.CSV>> GetAllAsync(
        CsvFilter csvFilter, 
        SortParams sortParams, 
        PageParams pageParams);
}
