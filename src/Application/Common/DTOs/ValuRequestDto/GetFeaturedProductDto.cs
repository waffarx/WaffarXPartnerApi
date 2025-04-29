using WaffarXPartnerApi.Application.Common.DTOs.Shared;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class GetFeaturedProductDto
{
    public int? PageNumber { get; set; } = 0;
    public int? Count { get; set; } = 50;
}
public class GetFeaturedProductRequestDto : BaseSharedDto
{
    public int? PageNumber { get; set; } = 0;
    public int? Count { get; set; } = 50;
}
