using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class ValuSearchResponseDto
{
    public List<ProductSearchResponseModel> Products { get; set; }
    public SearchFilterDto Filters { get; set; }

}

