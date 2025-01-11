using CsvParser.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CsvParser.Infrastructure.Persistence;

public class CSVParserDbContext : DbContext
{
    public CSVParserDbContext(DbContextOptions<CSVParserDbContext> options)
        : base(options)
    {
    }

    public CSVParserDbContext() { }

    public DbSet<CSV> CSVs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CSVParserDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
