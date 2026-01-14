namespace eBoardAPI.Models;

public class PagedDto<T>
{
    public IEnumerable<T> Data { get; set; } = [];
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
}