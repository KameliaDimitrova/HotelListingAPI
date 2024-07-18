namespace HotelListingAPI.Models;

public class QueryParametersRequestModel
{
    private int pageSize = 10;

    public int StartIndex { get; set; }

    public int PageSize
    {
        get
        {
            return pageSize;
        }
        set
        {
            pageSize = value;   
        }
    }
}
