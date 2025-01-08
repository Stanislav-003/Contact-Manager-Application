namespace CsvParser.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    ICSVRepository Csvs { get; }

    Task CompleteAsync();
}
