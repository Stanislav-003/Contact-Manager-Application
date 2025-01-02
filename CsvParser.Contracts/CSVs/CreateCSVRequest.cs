namespace CsvParser.Contracts.CSVs;

public record CreateCSVRequest(
    string Name,
    DateTime BirthDate,
    bool IsMarried,
    string Phone,
    decimal Salary);
