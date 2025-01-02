using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Infrastructure.Persistence;
using CsvParser.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CsvParser.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services)
    {
        services.AddDbContext<CSVParserDbContext>(options => 
            options.UseSqlServer("Data Source=STANISLAV003\\SQLEXPRESS;Initial Catalog=TestCsvParser;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));

        services.AddScoped<ICSVRepository, CSVRepository>();

        return services;
    }
}
