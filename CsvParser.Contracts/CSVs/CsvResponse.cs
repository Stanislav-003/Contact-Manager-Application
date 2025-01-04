namespace CsvParser.Contracts.CSVs;

public record CsvResponse(
        Guid Id,
        string Name,
        DateTime BirthDate,
        bool IsMarried,
        string Phone,
        decimal Salary);
