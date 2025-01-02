namespace CsvParser.Domain.Models;

public class CSV
{
    public Guid Id { get; private set; } 
    public string Name { get; private set; } = null!;
    public DateTime BirthDate { get; private set; }
    public bool IsMarried { get; private set; }
    public string Phone { get; private set; } = null!;
    public decimal Salary { get; private set; }

    public CSV() { }

    private CSV(
        Guid id,
        string name,
        DateTime birthDate,
        bool isMarried,
        string phone,
        decimal salary)
    {
        Id = id;
        Name = name;
        BirthDate = birthDate;
        IsMarried = isMarried;
        Phone = phone;
        Salary = salary;
    }

    public static CSV Create(
        string name,
        DateTime birthDate,
        bool isMarried,
        string phone,
        decimal salary)
    {
        return new(
            Guid.NewGuid(),
            name,
            birthDate,
            isMarried,
            phone,
            salary);
    }

    public void Update(string name, DateTime birthDate, bool isMarried, string phone, decimal salary)
    {
        Name = name;
        BirthDate = birthDate;
        IsMarried = isMarried;
        Phone = phone;
        Salary = salary;
    }
}
