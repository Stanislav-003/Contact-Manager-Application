using System.ComponentModel.DataAnnotations;

namespace CsvParser.Web.Models;

public class UploadCsvViewModel
{
    [Required]
    [DataType(DataType.Upload)]
    public IFormFile CsvFile { get; set; }
}
