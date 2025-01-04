namespace CsvParser.Contracts.ViewModel;

public class CSVViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsMarried { get; set; }
    public string Phone { get; set; }
    public decimal Salary { get; set; }
}
