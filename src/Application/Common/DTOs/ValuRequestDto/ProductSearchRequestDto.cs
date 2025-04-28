using WaffarXPartnerApi.Application.Common.DTOs.Shared;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class ProductSearchRequestDto : SearchBaseQueryDto
{
    public Guid? ClientApiId { get; set; }
    public FilterDto Filter { get; set; }
}
public class FilterDto
{
    public List<int> Stores { get; set; }
    public string Brands { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
}
