using CsvParser.Contracts.CSVs;
using CsvParser.Web.Interfaces;

namespace CsvParser.Web.Services;

public class CsvService : ICsvService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CsvService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<(CreateCSVRequest Record, string Error)>> SaveRecordsAsync(List<CreateCSVRequest> records)
    {
        var client = _httpClientFactory.CreateClient("CsvApi");
        var failedRecords = new List<(CreateCSVRequest Record, string Error)>();

        foreach (var record in records)
        {
            var response = await client.PostAsJsonAsync("CSV", record);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                failedRecords.Add((record, error));
            }
        }

        return failedRecords;
    }
}
