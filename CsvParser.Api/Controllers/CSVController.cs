using CsvParser.Application.CSV.Commands.CreateCSV;
using CsvParser.Application.CSV.Commands.DeleteCSV;
using CsvParser.Application.CSV.Commands.UpdateCSV;
using CsvParser.Application.CSV.Queries.Csv;
using CsvParser.Application.CSV.Queries.ListCsvs;
using CsvParser.Contracts.CSVs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CsvParser.Api.Controllers;

[Route("CSV")]
public class CSVController : ApiController
{
    private readonly ISender _mediator;

    public CSVController(ISender sender)
    {
        _mediator = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCsv(CreateCSVRequest request)
    {
        var command = new CreateCSVCommand(
            request.Name,
            request.BirthDate,
            request.IsMarried,
            request.Phone,
            request.Salary);

        var createCsvResult = await _mediator.Send(command);

        return createCsvResult.Match(
            csv => Ok(new CreateCSVResponse(
            csv.Id,
            csv.Name,
            csv.BirthDate,
            csv.IsMarried,
            csv.Phone,
            csv.Salary)),
            errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCsvs()
    {
        var query = new ListCsvsQuery();

        var csvsResult = await _mediator.Send(query);

        return csvsResult.Match(
            csvs => Ok(csvs.Select(csv => new AllCsvsResponse(
                csv.Id,
                csv.Name,
                csv.BirthDate,
                csv.IsMarried,
                csv.Phone,
                csv.Salary))),
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCsvById(Guid id)
    {
        var query = new CsvQuery(id); 

        var csvResult = await _mediator.Send(query);

        return csvResult.Match(
            csv => Ok(new CsvResponse( 
                csv.Id,
                csv.Name,
                csv.BirthDate,
                csv.IsMarried,
                csv.Phone,
                csv.Salary)),
            errors => Problem(errors)); 
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCsv(UpdateCSVRequest request)
    {
        var command = new UpdateCSVCommand(
            request.Id,
            request.Name,
            request.BirthDate,
            request.IsMarried,
            request.Phone,
            request.Salary);

        var updateCsvResult = await _mediator.Send(command);

        return updateCsvResult.Match(
            csv => Ok(new UpdateCSVResponse(
                csv.Id,
                csv.Name,
                csv.BirthDate,
                csv.IsMarried,
                csv.Phone,
                csv.Salary)),
            errors => Problem(errors));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCsv(Guid id)
    {
        var command = new DeleteCSVCommand(id);
        var result = await _mediator.Send(command);

        return result.Match(
            success => Ok(new { message = "CSV entry successfully deleted." }),
            error => Problem(error)
        );
    }
}