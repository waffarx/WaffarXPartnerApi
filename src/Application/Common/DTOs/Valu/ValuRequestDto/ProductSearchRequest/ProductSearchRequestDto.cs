using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;


namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
public class ProductSearchRequestDto : SearchBaseQueryDto
{
    public ProductSearchFilterDto Filter { get; set; }
}

public class ProductSearchDto : SearchBaseQueryModel
{
    public bool IsEnglish { get; set; } = true;
    public Guid ClientApiId { get; set; }
    public SearchFilterModel Filter { get; set; }
}
