using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.DeleteCSV;

public class DeleteCSVCommandHandler : IRequestHandler<DeleteCSVCommand, ErrorOr<bool>>
{
    private readonly ICSVRepository _csvRepository;

    public DeleteCSVCommandHandler(ICSVRepository csvRepository)
    {
        _csvRepository = csvRepository;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCSVCommand request, CancellationToken cancellationToken)
    {
        if (!await _csvRepository.ExistsAsync(request.Id))
        {
            return Errors.CSV.NotFound;
        }

        await _csvRepository.DeleteAsync(request.Id);

        return true;
    }
}
