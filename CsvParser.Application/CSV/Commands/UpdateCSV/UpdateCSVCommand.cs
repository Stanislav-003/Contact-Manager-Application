using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.UpdateCSV;

public record UpdateCSVCommand(
    Guid Id,
    string Name,
    DateTime BirthDate,
    bool IsMarried,
    string Phone,
    decimal Salary) : IRequest<ErrorOr<CsvParser.Domain.Models.CSV>>;
