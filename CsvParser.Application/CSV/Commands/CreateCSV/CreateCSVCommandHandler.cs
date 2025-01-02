using ErrorOr;
using MediatR;
using CsvParser.Domain.Models;
using CsvParser.Application.Common.Interfaces.Persistence;

namespace CsvParser.Application.CSV.Commands.CreateCSV;

public class CreateCSVCommandHandler : IRequestHandler<CreateCSVCommand, ErrorOr<CsvParser.Domain.Models.CSV>>
{
    private readonly ICSVRepository _csvRepository;

    public CreateCSVCommandHandler(ICSVRepository csvRepository)
    {
        _csvRepository = csvRepository;
    }

    public async Task<ErrorOr<Domain.Models.CSV>> Handle(CreateCSVCommand request, CancellationToken cancellationToken)
    {
        var csv = Domain.Models.CSV.Create(
            request.Name,
            request.BirthDate,
            request.IsMarried,
            request.Phone,
            request.Salary);

        await _csvRepository.AddAsync(csv);

        return csv;
    }
}
