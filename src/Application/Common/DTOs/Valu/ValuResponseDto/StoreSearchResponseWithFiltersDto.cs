using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
public class StoreSearchResponseWithFiltersDto
{
    public List<BaseProductSearchResultDto> Products { get; set; }
    public StoreSearchFilterDto Filters { get; set; }
}
public class StoreSearchResultWithFiltersDto
{
    public List<ProductSearchResponseModel> Products { get; set; }
    public StoreSearchFilterDto Filters { get; set; }
}

public class StoreSearchFilterDto 
{
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public List<string> Brands { get; set; }
    public List<string> Categories { get; set; }
    public List<OfferDto> Offers { get; set; }
}
