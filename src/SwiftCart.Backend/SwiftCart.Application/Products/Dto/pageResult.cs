using System;
namespace SwiftCart.Application.Dto;
public class PagedResult<T>
{
    public List<T>  Data { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
