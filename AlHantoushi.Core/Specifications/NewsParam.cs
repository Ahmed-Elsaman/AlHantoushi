namespace AlHantoushi.Core.Specifications;
public class NewsParam
{
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? Sort { get; set; }
    public int? Status { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
    public string? Date { get; set; }
    public string? Language { get; set; }
    public (DateTime? StartDate, DateTime? EndDate) GetDateRange()
    {
        if (string.IsNullOrWhiteSpace(Date)) return (null, null);

        var dateParts = Date.Split('/');
        if (dateParts.Length != 2) return (null, null);

        if (!int.TryParse(dateParts[0], out int month) || !int.TryParse(dateParts[1], out int year))
            return (null, null);
        var startDate = new DateTime(year, month, 1);

        var endDate = startDate.AddMonths(1).AddSeconds(-1);

        return (startDate, endDate);
    }

}
