using CsvParser.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CsvParser.Infrastructure.Persistence.Configurations;

public class CsvConfigurations : IEntityTypeConfiguration<CSV>
{
    public void Configure(EntityTypeBuilder<CSV> builder)
    {
        builder.ToTable("CSVs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasMaxLength(30);
        builder.Property(x => x.BirthDate).IsRequired();
        builder.Property(x => x.IsMarried).IsRequired();
        builder.Property(x => x.Phone).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Salary).IsRequired();

        //builder.HasCheckConstraint("CHK_BirthDate", "BirthDate < '1990-01-01'");
        //builder.HasCheckConstraint("CHK_Salary", "Salary >= 500");
    }
}