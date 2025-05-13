using WaffarXPartnerApi.Application.Common.DTOs.Shared;
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresWithProductsRequest;
public class GetStoresWithProductsDto : PaginationRequestDto
{
    public int ProductsCount { get; set; }
}

public class GetStoresWithProductsRequestDto  : BaseSharedDto
{
    public int ProductLimit { get; set; }
    public int PageNumber { get; set; }
    public int ItemCount { get; set; }
}
