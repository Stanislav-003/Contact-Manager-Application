using Microsoft.AspNetCore.Mvc;

namespace CsvParser.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Csv");
    }
}
