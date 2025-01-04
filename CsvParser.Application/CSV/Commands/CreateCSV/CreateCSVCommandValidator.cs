using FluentValidation;

namespace CsvParser.Application.CSV.Commands.CreateCSV;

public class CreateCSVCommandValidator : AbstractValidator<CreateCSVCommand>
{
    public CreateCSVCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        RuleFor(x => x.BirthDate).NotEmpty().LessThan(new DateTime(1990, 1, 1));
        //RuleFor(x => x.IsMarried).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Salary).NotEmpty().GreaterThanOrEqualTo(500);
    }
}
