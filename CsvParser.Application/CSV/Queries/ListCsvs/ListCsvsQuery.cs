using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Queries.ListCsvs;

public record ListCsvsQuery : IRequest<ErrorOr<List<CsvParser.Domain.Models.CSV>>>;