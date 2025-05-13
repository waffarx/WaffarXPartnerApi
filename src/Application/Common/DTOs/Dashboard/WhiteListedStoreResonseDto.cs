using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
public class WhiteListedStoreResonseDto : StoreDto
{
    public int Rank { get; set; }
    public bool IsFeatured { get; set; }
}

