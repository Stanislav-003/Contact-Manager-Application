using CsvParser.Api.Common.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CsvParser.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, CSVParserProblemDetailsFactory>();
        return services;
    }
}
