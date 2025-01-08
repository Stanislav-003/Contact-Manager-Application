using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.DeleteCSV;

public class DeleteCSVCommandHandler : IRequestHandler<DeleteCSVCommand, ErrorOr<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCSVCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCSVCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Csvs.ExistsAsync(request.Id))
        {
            return Errors.CSV.NotFound;
        }

        await _unitOfWork.Csvs.DeleteAsync(request.Id);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
