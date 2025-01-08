using CsvParser.Contracts.CSVs;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CsvParser.Web.Services;

public class CsvService : ICsvService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CsvService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<OperationResult<IEnumerable<CsvViewModel>>> GetAllAsync()
    {
        var client = _httpClientFactory.CreateClient("CsvApi");
        var response = await client.GetAsync("CSV");

        if (response.IsSuccessStatusCode)
        {
            var records = await response.Content.ReadFromJsonAsync<List<CsvViewModel>>();
            return OperationResult<IEnumerable<CsvViewModel>>.Succeeded(records ?? Enumerable.Empty<CsvViewModel>());
        }

        return OperationResult<IEnumerable<CsvViewModel>>.Failed($"Failed to get records. Status: {response.StatusCode}");
    }

    public async Task<OperationResult<List<CreateCSVRequest>>> SaveRecordsAsync(List<CreateCSVRequest> records)
    {
        if (records == null || records.Count == 0)
        {
            return OperationResult<List<CreateCSVRequest>>.Failed("No records to save.");
        }

        var client = _httpClientFactory.CreateClient("CsvApi");

        foreach (var record in records)
        {
            var response = await client.PostAsJsonAsync("CSV", record);

            if (!response.IsSuccessStatusCode)
            {
                var validationErrors = await response.Content.ReadFromJsonAsync<ApiValidationResponse>();

                var errorMessages = validationErrors?.Errors
                    .SelectMany(e => e.Value)
                    .ToList();

                return OperationResult<List<CreateCSVRequest>>.Failed(string.Join(Environment.NewLine, errorMessages!));
            }
        }

        return OperationResult<List<CreateCSVRequest>>.Succeeded(records);
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

        var validationErrors = await response.Content.ReadFromJsonAsync<ApiValidationResponse>();
        var errorMessages = validationErrors?.Errors
            .SelectMany(e => e.Value)
            .ToList();

        return OperationResult<CsvViewModel>.Failed(string.Join(Environment.NewLine, errorMessages!));
    }


    public async Task<OperationResult<CsvViewModel>> GetByIdAsync(Guid id)
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

        var validationErrors = await response.Content.ReadFromJsonAsync<ApiValidationResponse>();

        if (validationErrors != null && validationErrors.Errors != null)
        {
            var errorMessages = validationErrors.Errors
                .SelectMany(e => e.Value)
                .ToList();

            if (errorMessages.Any())
            {
                return OperationResult<CsvViewModel>.Failed(string.Join(Environment.NewLine, errorMessages));
            }
        }

        return OperationResult<CsvViewModel>.Failed($"Failed to get the record with ID: {id}. Response code: {response.StatusCode}");
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

        return OperationResult<bool>.Failed($"Failed to delete record with ID: {id}. Response code: {response.StatusCode}");
    }
}
