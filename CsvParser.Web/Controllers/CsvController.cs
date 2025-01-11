using CsvParser.Web.Extensions;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Sorting;

namespace CsvParser.Web.Controllers;

public class CsvController : Controller
{
    private readonly ICsvService _csvService;

    public CsvController(ICsvService csvService)
    {
        _csvService = csvService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? name, string? orderBy, string? sortDirection, int? page, int? pageSize)
    {
        var result = await _csvService.GetAllAsync(name, orderBy, sortDirection, page, pageSize);
        if (result.Success)
        {
            ViewBag.TotalCount = result.Data!.TotalCount;
            return View(result.Data!.Data);
        }
        return View(Enumerable.Empty<CsvViewModel>());
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

        var result = await CsvHelperExtensions.ParseCsvFile(file);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View();
        }

        var failedRecords = await _csvService.SaveRecordsAsync(result.Data!);

        if (!failedRecords.Success)
        {
            var errors = failedRecords.Error!.Split(Environment.NewLine);
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(result.Data);
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
