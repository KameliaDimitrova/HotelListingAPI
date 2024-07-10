namespace HotelListingAPI.Models;

public class PagedResponseModel<T>
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int RecordNumber {  get; set; }

    public List<T> Items { get; set; } = [];
}
