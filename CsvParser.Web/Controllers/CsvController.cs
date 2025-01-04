using CsvParser.Web.Extensions;
using CsvParser.Web.Interfaces;
using CsvParser.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CsvParser.Web.Controllers;

public class CsvController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICsvService _csvService;
    private readonly ILogger<CsvController> _logger;

    public CsvController(IHttpClientFactory httpClientFactory, ICsvService csvService, ILogger<CsvController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _csvService = csvService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CsvApi");
        var response = await client.GetAsync("CSV");
        var csvRecords = await response.Content.ReadFromJsonAsync<List<CsvViewModel>>();
        return View(csvRecords);
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
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"An error occurred while processing the file: {ex.Message}");
            return View();
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CsvApi");
        var response = await client.GetAsync($"CSV/{id}"); 

        if (response.IsSuccessStatusCode)
        {
            var csv = await response.Content.ReadFromJsonAsync<CsvViewModel>();  
            return View(csv);  
        }

        return NotFound();  
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CsvViewModel model)
    {
        // Добавьте логирование для проверки значения
        _logger.LogInformation($"IsMarried value: {model.IsMarried}");

        if (ModelState.IsValid)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CsvApi");

                var response = await client.PutAsJsonAsync("CSV", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                // Добавим отладочную информацию
                var responseContent = await response.Content.ReadAsStringAsync();

                ModelState.AddModelError("", $"Failed to update the record. {responseContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }
        }
        else
        {
            // Логируем ошибки ModelState
            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError($"Model Error: {modelError.ErrorMessage}");
            }
        }

        return View(model);  
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CsvApi");
        var response = await client.DeleteAsync($"CSV/{id}");
        if (response.IsSuccessStatusCode) return RedirectToAction("Index");

        return BadRequest("Failed to delete the record.");
    }
}
