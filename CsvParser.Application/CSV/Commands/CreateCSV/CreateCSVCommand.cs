using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.CreateCSV;

public record CreateCSVCommand(
    string Name,
    DateTime BirthDate,
    bool IsMarried,
    string Phone,
    decimal Salary) : IRequest<ErrorOr<CsvParser.Domain.Models.CSV>>;
