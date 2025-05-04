using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;
public class GetStoresRequestDto : PaginationRequestDto
{
}
public class GetStoresDto : BaseSharedDto
{
    public int PageNumber { get; set; }
    public int ItemCount { get; set; }
}
