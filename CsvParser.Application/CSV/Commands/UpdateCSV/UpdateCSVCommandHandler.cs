using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CsvParser.Application.CSV.Commands.UpdateCSV;

public class UpdateCSVCommandHandler : IRequestHandler<UpdateCSVCommand, ErrorOr<CsvParser.Domain.Models.CSV>>
{
    private readonly ICSVRepository _csvRepository;

    public UpdateCSVCommandHandler(ICSVRepository csvRepository)
    {
        _csvRepository = csvRepository;
    }

    public async Task<ErrorOr<CsvParser.Domain.Models.CSV>> Handle(UpdateCSVCommand request, CancellationToken cancellationToken)
    {
        var existingCsv = await _csvRepository.GetByIdAsync(request.Id);

        //if (!await _csvRepository.ExistsAsync(request.Id))
        //{
        //    return Errors.CSV.NotFound;
        //}

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

        await _csvRepository.UpdateAsync(existingCsv);

        return existingCsv;
    }
}
