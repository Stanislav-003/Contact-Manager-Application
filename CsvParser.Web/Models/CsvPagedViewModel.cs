using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Sorting;

namespace CsvParser.Web.Models;

public class CsvPagedViewModel
{
    public List<CsvViewModel> Data { get; set; } = new List<CsvViewModel>();
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public CsvFilter Filter { get; set; } = new CsvFilter();
    public SortParams Sort { get; set; } = new SortParams();
}
