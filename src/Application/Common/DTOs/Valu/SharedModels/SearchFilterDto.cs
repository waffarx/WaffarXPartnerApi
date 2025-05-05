namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
public class SearchFilterDto : FilterBase
{
    public List<string> Brands { get; set; }
    public List<OfferDto> Offers { get; set; }
    public List<StoreResponseDto> Stores { get; set; }

}
