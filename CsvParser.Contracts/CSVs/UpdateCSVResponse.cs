namespace CsvParser.Contracts.CSVs;

public record UpdateCSVResponse(
    Guid Id,
    string Name,
    DateTime BirthDate,
    bool IsMarried,
    string Phone,
    decimal Salary);
