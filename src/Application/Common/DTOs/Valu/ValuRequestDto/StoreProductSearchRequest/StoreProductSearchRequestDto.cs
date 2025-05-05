using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
public class StoreProductSearchRequestDto : FilterBase
{
    public Guid StoreId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Category { get; set; }

}
