using CsvHelper.Configuration;
using CsvHelper;
using CsvParser.Contracts.CSVs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Net.Http;

namespace CsvParser.Web.Extensions;

public static class CsvHelperExtensions
{
    public static async Task<List<CreateCSVRequest>> ParseCsvFile(IFormFile file)
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
            throw new InvalidOperationException("Failed to read CSV headers.");

        ValidateHeaders(csv.HeaderRecord!);

        var records = new List<CreateCSVRequest>();
        var rowNumber = 1;

        while (csv.Read())
        {
            rowNumber++;
            records.Add(ParseRecord(csv, rowNumber));
        }

        return records;
    }

    private static void ValidateHeaders(string[] headers)
    {
        var requiredHeaders = new[] { "Name", "BirthDate", "IsMarried", "Phone", "Salary" };
        var missingHeaders = requiredHeaders.Except(headers, StringComparer.OrdinalIgnoreCase);

        if (missingHeaders.Any())
            throw new InvalidOperationException($"Missing required columns: {string.Join(", ", missingHeaders)}");
    }

    private static CreateCSVRequest ParseRecord(CsvReader csv, int rowNumber)
    {
        try
        {
            return new CreateCSVRequest(
                csv.GetField<string>("Name")!,
                csv.GetField<DateTime>("BirthDate"),
                csv.GetField<bool>("IsMarried"),
                csv.GetField<string>("Phone")!,
                csv.GetField<decimal>("Salary")
            );
        }
        catch (CsvHelper.TypeConversion.TypeConverterException ex)
        {
            throw new InvalidOperationException($"Data conversion error in row {rowNumber}: {ex.Message}");
        }
    }

    public static void AddSaveErrorsToModelState(
        List<(CreateCSVRequest record, string error)> failedRecords, ModelStateDictionary modelState)
    {
        foreach (var (record, error) in failedRecords)
        {
            modelState.AddModelError("", $"Error saving record {record.Name}: {error}");
        }
    }
}
