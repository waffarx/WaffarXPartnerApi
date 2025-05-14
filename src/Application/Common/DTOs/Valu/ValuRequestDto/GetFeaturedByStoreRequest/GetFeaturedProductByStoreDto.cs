using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
public class GetFeaturedProductByStoreDto : PaginationRequestDto
{
    public string StoreId { get; set; }
}

public class GetFeaturedProductByStoreRequestDto : BaseSharedDto
{
    public Guid? StoreId { get; set; }
    public int? PageNumber { get; set; } = 1;
    public int? Count { get; set; } = 50;
}
