using CsvParser.Contracts.CSVs;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.Extensions.Logging;

namespace CsvParser.Web.Services;

public class CsvService : ICsvService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CsvService> _logger;

    public CsvService(IHttpClientFactory httpClientFactory, ILogger<CsvService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<CsvViewModel>> GetAllAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var response = await client.GetAsync("CSV");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CsvViewModel>>()
                    ?? new List<CsvViewModel>();
            }

            _logger.LogError("Failed to get CSV records. Status code: {StatusCode}", response.StatusCode);
            return new List<CsvViewModel>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all CSV records");
            throw;
        }
    }

    public async Task<CsvViewModel?> GetByIdAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var response = await client.GetAsync($"CSV/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CsvViewModel>();
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogInformation("CSV record with ID {Id} not found", id);
                return null;
            }

            _logger.LogError("Failed to get CSV record. ID: {Id}, Status code: {StatusCode}",
                id, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting CSV record with ID {Id}", id);
            throw;
        }
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

    public async Task<(bool Success, string? Error)> UpdateAsync(CsvViewModel model)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var updateRequest = new UpdateCSVRequest(
                model.Id,
                model.Name,
                model.BirthDate,
                model.IsMarried,
                model.Phone,
                model.Salary);

            var response = await client.PutAsJsonAsync("CSV", updateRequest);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to update CSV record. ID: {Id}, Error: {Error}",
                model.Id, error);
            return (false, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating CSV record with ID {Id}",
                model.Id);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var response = await client.DeleteAsync($"CSV/{id}");

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to delete CSV record. ID: {Id}, Error: {Error}",
                id, error);
            return (false, error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting CSV record with ID {Id}",
                id);
            return (false, ex.Message);
        }
    }
}
