using CsvParser.Contracts.CSVs;

namespace CsvParser.Web.Interfaces;

public interface ICsvService
{
    Task<List<(CreateCSVRequest Record, string Error)>> SaveRecordsAsync(List<CreateCSVRequest> records);
}
