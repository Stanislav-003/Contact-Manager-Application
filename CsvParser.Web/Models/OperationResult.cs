namespace CsvParser.Web.Models;

public class OperationResult<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public string? Error { get; }

    private OperationResult(bool success, T? data, string? error)
    {
        Success = success;
        Data = data;
        Error = error;
    }

    public static OperationResult<T> Succeeded(T data) =>
        new(true, data, null);

    public static OperationResult<T> Failed(string error) =>
        new(false, default, error);
}
