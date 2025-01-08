using CsvParser.Domain.Models;
using CsvParser.Domain.Abstractions;

namespace CsvParser.Application.Common.Interfaces.Persistence;

public interface ICSVRepository : IGenericRepository<CsvParser.Domain.Models.CSV>
{
}
