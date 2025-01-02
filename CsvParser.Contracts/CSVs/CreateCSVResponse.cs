namespace CsvParser.Contracts.CSVs;

public record CreateCSVResponse(
        Guid Id,
        string Name,
        DateTime BirthDate,
        bool IsMarried,
        string Phone,
        decimal Salary);
