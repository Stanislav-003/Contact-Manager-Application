using CsvParser.Contracts.CSVs;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<OperationResult<IEnumerable<CsvViewModel>>> GetAllAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var response = await client.GetAsync("CSV");

            if (response.IsSuccessStatusCode)
            {
                var records = await response.Content.ReadFromJsonAsync<List<CsvViewModel>>();
                return OperationResult<IEnumerable<CsvViewModel>>.Succeeded(records ?? Enumerable.Empty<CsvViewModel>());
            }

            _logger.LogError("Failed to get CSV records. Status code: {StatusCode}", response.StatusCode);
            return OperationResult<IEnumerable<CsvViewModel>>.Failed($"Failed to get records. Status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all CSV records");
            return OperationResult<IEnumerable<CsvViewModel>>.Failed(ex.Message);
        }
    }

    public async Task<OperationResult<CsvViewModel>> GetByIdAsync(Guid id)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var response = await client.GetAsync($"CSV/{id}");

            if (response.IsSuccessStatusCode)
            {
                var record = await response.Content.ReadFromJsonAsync<CsvViewModel>();
                return record != null
                    ? OperationResult<CsvViewModel>.Succeeded(record)
                    : OperationResult<CsvViewModel>.Failed("Record not found");
            }

            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get CSV record. ID: {Id}, Status: {Status}, Error: {Error}",
                id, response.StatusCode, error);
            return OperationResult<CsvViewModel>.Failed(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting CSV record with ID {Id}", id);
            return OperationResult<CsvViewModel>.Failed(ex.Message);
        }
    }

    public async Task<OperationResult<List<(CreateCSVRequest Record, string Error)>>> SaveRecordsAsync(List<CreateCSVRequest> records)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("CsvApi");
            var failedRecords = new List<(CreateCSVRequest Record, string Error)>();

            foreach (var record in records)
            {
                var response = await client.PostAsJsonAsync("CSV", record);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to save CSV record. Name: {Name}, Status: {Status}, Error: {Error}",
                        record.Name, response.StatusCode, error);
                    failedRecords.Add((record, error));
                }
            }

            return failedRecords.Any()
                ? OperationResult<List<(CreateCSVRequest Record, string Error)>>.Failed("Some records failed to save")
                : OperationResult<List<(CreateCSVRequest Record, string Error)>>.Succeeded(failedRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving CSV records");
            return OperationResult<List<(CreateCSVRequest Record, string Error)>>.Failed(ex.Message);
        }
    }

    public async Task<OperationResult<CsvViewModel>> UpdateAsync(CsvViewModel model)
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
            var updatedRecord = await response.Content.ReadFromJsonAsync<CsvViewModel>();
            return OperationResult<CsvViewModel>.Succeeded(updatedRecord!);
        }

        //if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //{

        //}

        var validationErrors = await response.Content.ReadFromJsonAsync<ApiValidationResponse>();
        var errorMessages = validationErrors?.Errors
            .SelectMany(e => e.Value)
            .ToList();

        return OperationResult<CsvViewModel>.Failed(
            string.Join(Environment.NewLine, errorMessages!));
    }

    public async Task<OperationResult<bool>> DeleteAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CsvApi");
        var response = await client.DeleteAsync($"CSV/{id}");

        if (response.IsSuccessStatusCode)
        {
            return OperationResult<bool>.Succeeded(true);
        }

        var validationErrors = await response.Content.ReadFromJsonAsync<ApiValidationResponse>();
        //var errorMessages = validationErrors?.Errors
        //    .SelectMany(e => e.Value)
        //    .ToList();

        //if (errorMessages != null && errorMessages.Any())
        //{
        //    return OperationResult<bool>.Failed(string.Join(Environment.NewLine, errorMessages));
        //}

        // Проверяем, что validationErrors не равен null, и ошибки существуют
        if (validationErrors != null && validationErrors.Errors != null)
        {
            var errorMessages = validationErrors.Errors
                .SelectMany(e => e.Value)
                .ToList();

            if (errorMessages.Any())
            {
                return OperationResult<bool>.Failed(string.Join(Environment.NewLine, errorMessages));
            }
        }

        return OperationResult<bool>.Failed($"Не удалось удалить запись с ID: {id}. Код ответа: {response.StatusCode}");
    }
}
