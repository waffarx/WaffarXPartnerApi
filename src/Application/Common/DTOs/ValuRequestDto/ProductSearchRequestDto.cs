using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class ProductSearchRequestDto : SearchBaseQueryDto
{
    public FilterDto Filter { get; set; }
}
public class FilterDto
{
    public List<int> Stores { get; set; }
    public string Brands { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public List<OfferDto> Offers { get; set; }

}

public class ProductSearchDto : SearchBaseQueryDto 
{
    public bool IsEnglish { get; set; } = true;
    public Guid? ClientApiId { get; set; }
    public FilterDto Filter { get; set; }
}
