using WaffarXPartnerApi.Application.Common.DTOs.Shared;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class GetStoreDto : BaseSharedDto
{
    public Guid StoreId { get; set; }

}
