using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Sorting;
using CsvParser.Contracts.CSVs;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Queries.ListCsvs;

public record ListCsvsQuery(
    CsvFilter CsvFilter,
    SortParams SortParams,
    PageParams PageParams) : IRequest<ErrorOr<CsvPagedResult<CsvParser.Domain.Models.CSV>>>;