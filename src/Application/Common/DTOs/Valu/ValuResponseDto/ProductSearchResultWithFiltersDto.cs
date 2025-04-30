using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class ProductSearchResultWithFiltersDto
{
    public List<ProductSearchResultDto> Products { get; set; }
    public SearchFilterDto Filters { get; set; }

}
