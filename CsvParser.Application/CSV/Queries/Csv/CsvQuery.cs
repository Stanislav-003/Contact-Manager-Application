using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Queries.Csv;

public record CsvQuery(Guid Id) : IRequest<ErrorOr<CsvParser.Domain.Models.CSV>>;
