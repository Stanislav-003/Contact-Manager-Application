namespace CsvParser.Contracts.CSVs;

public class CsvPagedResult<T>
{
    public List<T> Data { get; }
    public int TotalCount { get; }

    public CsvPagedResult(List<T> data, int totalCount)
    {
        Data = data;
        TotalCount = totalCount;
    }
}
