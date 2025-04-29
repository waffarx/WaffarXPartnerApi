using WaffarXPartnerApi.Application.Common.DTOs.Shared;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class GetStoresRequestDto
{
    public int PageNumber { get; set; }
    public int ItemCount { get; set; }
}
public class GetStoresDto : BaseSharedDto
{
    public int PageNumber { get; set; }
    public int ItemCount { get; set; }
}
