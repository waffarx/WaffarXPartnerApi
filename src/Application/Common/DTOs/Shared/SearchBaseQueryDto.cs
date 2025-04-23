namespace WaffarXPartnerApi.Application.Common.DTOs.Shared;
public class SearchBaseQueryDto
{
    public string SearchText { get; set; }
    public int PageNumber { get; set; }
    public int ItemCount { get; set; }
    public bool IsEnglish { get; set; }
}
