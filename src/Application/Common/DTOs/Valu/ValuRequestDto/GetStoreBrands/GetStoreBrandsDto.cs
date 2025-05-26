using WaffarXPartnerApi.Application.Common.DTOs.Shared;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoreBrands;
public class GetStoreBrandsDto
{
    public string SearchText { get; set; }
    public string StoreId { get; set; }
}


public class GetStoreBrandsRequestDto : BaseSharedDto
{
    public string SearchText { get; set; }
    public Guid StoreId { get; set; }
}
