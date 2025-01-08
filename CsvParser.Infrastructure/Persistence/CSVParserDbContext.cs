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

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        //optionsBuilder.UseSqlServer("Data Source=STANISLAV003\\SQLEXPRESS;Initial Catalog=TestCsvParser;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
    //        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    //    }
    //}
}
