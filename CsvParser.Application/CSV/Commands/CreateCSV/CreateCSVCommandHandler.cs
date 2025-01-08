using CsvParser.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.CreateCSV;

public class CreateCSVCommandHandler : IRequestHandler<CreateCSVCommand, ErrorOr<CsvParser.Domain.Models.CSV>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCSVCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Domain.Models.CSV>> Handle(CreateCSVCommand request, CancellationToken cancellationToken)
    {
        var csv = Domain.Models.CSV.Create(
            request.Name,
            request.BirthDate,
            request.IsMarried,
            request.Phone,
            request.Salary);

        await _unitOfWork.Csvs.AddAsync(csv);
        await _unitOfWork.CompleteAsync();

        return csv;
    }
}
