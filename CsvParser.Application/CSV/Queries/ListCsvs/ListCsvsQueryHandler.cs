using CsvParser.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Queries.ListCsvs;

public class ListCsvsQueryHandler : IRequestHandler<ListCsvsQuery, ErrorOr<List<CsvParser.Domain.Models.CSV>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ListCsvsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<List<CsvParser.Domain.Models.CSV>>> Handle(ListCsvsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Csvs.GetAllAsync();
    }
}
