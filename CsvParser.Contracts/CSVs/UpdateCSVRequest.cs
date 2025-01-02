namespace CsvParser.Contracts.CSVs;

public record UpdateCSVRequest(
    Guid Id,
    string Name,
    DateTime BirthDate,
    bool IsMarried,
    string Phone,
    decimal Salary);
