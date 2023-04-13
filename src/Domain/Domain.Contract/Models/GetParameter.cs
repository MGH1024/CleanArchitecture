namespace Domain.Contract.Models;

public class GetParameter
{
    const int _maxPageSize = 20;
    private int _pageSize = 10;
    private int _pageNumber = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value <= 1) ? 0 : value - 1;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }

    public string OrderBy { get; set; } = "Id";
    public string OrderType { get; set; } = "desc";
    public string Search { get; set; } = "";
}