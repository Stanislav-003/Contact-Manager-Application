using CsvParser.Application.Common.Filters;
using CsvParser.Application.Common.Sorting;
using CsvParser.Contracts.CSVs;
using CsvParser.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CsvParser.Infrastructure.Extensions;
 
public static class CsvExtensions
{
    public static IQueryable<CSV> Filter(this IQueryable<CSV> query, CsvFilter? csvFilter)
    {
        if (csvFilter == null) return query;

        if (!string.IsNullOrEmpty(csvFilter.Name))
            query = query.Where(x => x.Name.Contains(csvFilter.Name)); 

        return query;
    }

    public static IQueryable<CSV> Sort(this IQueryable<CSV> query, SortParams sortParams)
    { 
        if (sortParams.SortDirection == SortDirection.Descending)
            return query.OrderByDescending(GetKeySelector(sortParams.OrderBy));

        return query.OrderBy(GetKeySelector(sortParams.OrderBy));
    }

    public static async Task<CsvPagedResult<CSV>> ToPageAsync(this IQueryable<CSV> query, PageParams pageParams)
    {
        var count = await query.CountAsync();
        if (count == 0)
            return new CsvPagedResult<CSV>([], 0);

        var page = pageParams.Page ?? 1;
        var pageSize = pageParams.PageSize ?? 4;

        var skip = (page - 1) * pageSize;

        var result = await query.Skip(skip).Take(pageSize).ToListAsync();

        return new CsvPagedResult<CSV>(result, count);
    }

    private static Expression<Func<CSV, object>> GetKeySelector(string? orderBy)
    {
        if (string.IsNullOrEmpty(orderBy))
            return x => x.Name;

        return orderBy switch
        {
            nameof(CSV.Salary) => x => x.Salary,
            nameof(CSV.BirthDate) => x => x.BirthDate,
            _ => x => x.Name
        };
    }
}
