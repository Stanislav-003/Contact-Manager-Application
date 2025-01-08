using CsvHelper.Configuration;
using CsvHelper;
using CsvParser.Contracts.CSVs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Net.Http;
using CsvParser.Web.Models;

namespace CsvParser.Web.Extensions;

public static class CsvHelperExtensions
{
    public static async Task<OperationResult<List<CreateCSVRequest>>> ParseCsvFile(IFormFile file)
    {
        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            if (!csv.Read() || !csv.ReadHeader())
                return OperationResult<List<CreateCSVRequest>>.Failed("Failed to read CSV headers.");

            var headerValidationResult = ValidateHeaders(csv.HeaderRecord!);
            if (!headerValidationResult.Success)
            {
                return OperationResult<List<CreateCSVRequest>>.Failed(headerValidationResult.Error!);
            }

            var records = new List<CreateCSVRequest>();
            var rowNumber = 1;

            while (csv.Read())
            {
                rowNumber++;
                var recordResult = ParseRecord(csv, rowNumber);
                if (!recordResult.Success)
                {
                    return OperationResult<List<CreateCSVRequest>>.Failed(recordResult.Error!);
                }
                records.Add(recordResult.Data!);
            }

            return OperationResult<List<CreateCSVRequest>>.Succeeded(records);
        }
        catch (Exception ex)
        {
            return OperationResult<List<CreateCSVRequest>>.Failed($"Unexpected error: {ex.Message}");
        }
    }

    private static OperationResult<string> ValidateHeaders(string[] headers)
    {
        var requiredHeaders = new[] { "Name", "BirthDate", "IsMarried", "Phone", "Salary" };
        var missingHeaders = requiredHeaders.Except(headers, StringComparer.OrdinalIgnoreCase);

        if (missingHeaders.Any())
        {
            return OperationResult<string>.Failed($"Missing required columns: {string.Join(", ", missingHeaders)}");
        }

        return OperationResult<string>.Succeeded("Headers validated successfully.");
    }

    private static OperationResult<CreateCSVRequest> ParseRecord(CsvReader csv, int rowNumber)
    {
        try
        {
            return OperationResult<CreateCSVRequest>.Succeeded(new CreateCSVRequest(
                csv.GetField<string>("Name")!,
                csv.GetField<DateTime>("BirthDate"),
                csv.GetField<bool>("IsMarried"),
                csv.GetField<string>("Phone")!,
                csv.GetField<decimal>("Salary")
            ));
        }
        catch (CsvHelper.TypeConversion.TypeConverterException ex)
        {
            return OperationResult<CreateCSVRequest>.Failed($"Data conversion error in row {rowNumber}: {ex.Message}");
        }
    }
}
