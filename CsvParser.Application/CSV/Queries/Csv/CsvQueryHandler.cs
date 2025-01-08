using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Application.CSV.Queries.ListCsvs;
using CsvParser.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Queries.Csv;

public class CsvQueryHandler : IRequestHandler<CsvQuery, ErrorOr<CsvParser.Domain.Models.CSV>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CsvQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Domain.Models.CSV>> Handle(CsvQuery request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Csvs.ExistsAsync(request.Id))
        {
            return Errors.CSV.NotFound;
        }

        var csv = await _unitOfWork.Csvs.GetByIdAsync(request.Id);

        return csv!;
    }
}
