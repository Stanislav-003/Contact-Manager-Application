using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.UpdateCSV;

public class UpdateCSVCommandHandler : IRequestHandler<UpdateCSVCommand, ErrorOr<CsvParser.Domain.Models.CSV>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCSVCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<CsvParser.Domain.Models.CSV>> Handle(UpdateCSVCommand request, CancellationToken cancellationToken)
    {
        var existingCsv = await _unitOfWork.Csvs.GetByIdAsync(request.Id);

        if (existingCsv is null)
        {
            return Errors.CSV.NotFound;
        }

        existingCsv.Update(
            request.Name,
            request.BirthDate,
            request.IsMarried,
            request.Phone,
            request.Salary);

        await _unitOfWork.CompleteAsync();

        return existingCsv;
    }
}
