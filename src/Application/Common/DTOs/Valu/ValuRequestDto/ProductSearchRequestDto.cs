using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class ProductSearchRequestDto : SearchBaseQueryDto
{
    public SearchFilterModel Filter { get; set; }
}

public class ProductSearchDto : SearchBaseQueryModel
{
    public bool IsEnglish { get; set; } = true;
    public Guid? ClientApiId { get; set; }
    public SearchFilterModel Filter { get; set; }
}
