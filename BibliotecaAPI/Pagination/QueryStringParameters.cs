namespace BibliotecaAPI.Pagination;
public abstract class QueryStringParameters
{
    private int _pageSize = MAX_PAGE_SIZE;
    const int MAX_PAGE_SIZE = 50;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
    }
}