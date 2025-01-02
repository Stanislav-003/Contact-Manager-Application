namespace CsvParser.Contracts.CSVs;

public record AllCsvsResponse(
        Guid Id,
        string Name,
        DateTime BirthDate,
        bool IsMarried,
        string Phone,
        decimal Salary);
