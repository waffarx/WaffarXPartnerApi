using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedProductRequest;
public class GetFeaturedProductDto : PaginationRequestDto
{
}
public class GetFeaturedProductRequestDto : BaseSharedDto
{
    public int? PageNumber { get; set; } = 1;
    public int? Count { get; set; } = 50;
}
