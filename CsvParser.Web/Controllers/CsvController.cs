using CsvParser.Web.Extensions;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CsvParser.Web.Controllers;

public class CsvController : Controller
{
    private readonly ICsvService _csvService;

    public CsvController(ICsvService csvService)
    {
        _csvService = csvService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _csvService.GetAllAsync();
        return result.Success ? View(result.Data!.ToList()) : View(Enumerable.Empty<CsvViewModel>());
    }

    [HttpGet]
    public IActionResult Upload() => View();

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0) return View();

        var records = await CsvHelperExtensions.ParseCsvFile(file);
        if (!records.Any()) return View();

        var failedRecords = await _csvService.SaveRecordsAsync(records);
        if (failedRecords.Data!.Any()) return View();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _csvService.GetByIdAsync(id);
        return result.Success ? View(result.Data) : RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CsvViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _csvService.UpdateAsync(model);
        if (!result.Success)
        {
            var errors = result.Error!.Split(Environment.NewLine);
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _csvService.DeleteAsync(id);

        if (result.Success)
        {
            return RedirectToAction("Index");
        }

        return BadRequest(new { Error = result.Error });
    }
}
