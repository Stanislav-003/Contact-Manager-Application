using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Contracts.CSVs;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Queries.ListCsvs;

public class ListCsvsQueryHandler : IRequestHandler<ListCsvsQuery, ErrorOr<CsvPagedResult<CsvParser.Domain.Models.CSV>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ListCsvsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<CsvPagedResult<CsvParser.Domain.Models.CSV>>> Handle(
        ListCsvsQuery request, 
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Csvs.GetAllAsync(
            request.CsvFilter, 
            request.SortParams, 
            request.PageParams);
    }
}
