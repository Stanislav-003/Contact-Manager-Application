using CsvParser.Web.Extensions;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CsvParser.Web.Controllers;

public class CsvController : Controller
{
    private readonly ICsvService _csvService;
    private readonly ILogger<CsvController> _logger;

    public CsvController(ICsvService csvService, ILogger<CsvController> logger)
    {
        _csvService = csvService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var csvRecords = await _csvService.GetAllAsync();
            return View(csvRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching CSV records");
            TempData["ErrorMessage"] = "Failed to load CSV records. Please try again later.";
            return View(Enumerable.Empty<CsvViewModel>());
        }
    }

    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("", "The file is missing or empty.");
            return View();
        }

        try
        {
            var records = await CsvHelperExtensions.ParseCsvFile(file);

            if (!records.Any())
            {
                ModelState.AddModelError("", "The CSV file contains no data.");
                return View();
            }

            var failedRecords = await _csvService.SaveRecordsAsync(records);

            if (failedRecords.Any())
            {
                CsvHelperExtensions.AddSaveErrorsToModelState(failedRecords, ModelState);
                return View();
            }

            TempData["SuccessMessage"] = $"Successfully uploaded {records.Count} records.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing CSV file");
            ModelState.AddModelError("", $"An error occurred while processing the file: {ex.Message}");
            return View();
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var csv = await _csvService.GetByIdAsync(id);

            if (csv == null)
            {
                return NotFound();
            }

            return View(csv);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching CSV record with ID {Id}", id);
            TempData["ErrorMessage"] = "Failed to load the record. Please try again later.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CsvViewModel model)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError("Model Error: {ErrorMessage}", modelError.ErrorMessage);
            }
            return View(model);
        }

        try
        {
            var (success, error) = await _csvService.UpdateAsync(model);
            if (success)
            {
                TempData["SuccessMessage"] = "Record updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            // Разбираем ошибку из API и добавляем в ModelState
            if (!string.IsNullOrEmpty(error))
            {
                try
                {
                    // Предполагаем, что ошибка может прийти в формате JSON
                    var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(error);
                    if (problemDetails != null)
                    {
                        ModelState.AddModelError("", problemDetails.Title ?? problemDetails.Detail ?? "Failed to update the record.");
                    }
                    else
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                catch
                {
                    ModelState.AddModelError("", error);
                }
            }
            else
            {
                ModelState.AddModelError("", "Failed to update the record.");
            }

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating CSV record with ID {Id}", model.Id);
            ModelState.AddModelError("", "An error occurred while updating the record. Please try again.");
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var (success, error) = await _csvService.DeleteAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Record deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = error ?? "Failed to delete the record.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting CSV record with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the record.";
            return RedirectToAction(nameof(Index));
        }
    }
}
