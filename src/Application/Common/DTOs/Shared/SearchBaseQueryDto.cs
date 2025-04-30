namespace WaffarXPartnerApi.Application.Common.DTOs.Shared;
public class SearchBaseQueryDto : BaseQuery
{
    public int PageSize { get; set; }
}
public class SearchBaseQueryModel : BaseQuery
{
    public int ItemCount { get; set; }
}
public class BaseQuery
{
    public string SearchText { get; set; }
    public int PageNumber { get; set; }

}
