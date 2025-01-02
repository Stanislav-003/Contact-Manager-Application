using ErrorOr;

namespace CsvParser.Domain.Common.Errors;

public static class Errors
{
    public static class CSV
    {
        public static Error NotFound => Error.NotFound(
            code: "Csv.NotFound",
            description: "Csv with given ID does not exist");
    }
}
