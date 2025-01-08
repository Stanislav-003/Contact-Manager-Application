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
        if (file == null || file.Length == 0 || !file.ContentType.Contains("csv"))
        {
            ModelState.AddModelError(string.Empty, "Invalid file. Please upload a valid CSV file.");
            return View();
        }

        var records = await CsvHelperExtensions.ParseCsvFile(file);
        if (!records.Any())
        {
            ModelState.AddModelError(string.Empty, "No records found in the uploaded file.");
            return View();
        }

        var failedRecords = await _csvService.SaveRecordsAsync(records);

        if (!failedRecords.Success)
        {
            var errors = failedRecords.Error?.Split(Environment.NewLine) ?? new string[] { "Unknown error occurred." };
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(records);
        }

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

        if (result.Success)
        {
            return RedirectToAction(nameof(Index));
        }

        var errors = result.Error!.Split(Environment.NewLine);
        foreach (var error in errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _csvService.DeleteAsync(id);

        if (result.Success)
        {
            return RedirectToAction("Index");
        }

        return BadRequest(new { result.Error });
    }
}
