using CsvParser.Application.Common.Interfaces.Persistence;
using CsvParser.Infrastructure.Persistence;
using CsvParser.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CsvParser.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDbContext<CSVParserDbContext>(options =>
        //    options.UseSqlServer("Data Source=STANISLAV003\\SQLEXPRESS;Initial Catalog=TestCsvParser;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));

        services.AddDbContext<CSVParserDbContext>(options =>
           options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICSVRepository, CSVRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
