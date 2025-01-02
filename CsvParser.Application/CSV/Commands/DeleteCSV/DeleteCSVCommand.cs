using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.DeleteCSV;

public record DeleteCSVCommand(Guid Id) : IRequest<ErrorOr<bool>>;
