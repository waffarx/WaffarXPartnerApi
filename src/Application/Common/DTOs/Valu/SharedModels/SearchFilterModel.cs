namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
public class SearchFilterModel : FilterBase
{
    public string OfferId { get; set; }
    public List<StoreRequestDto> Stores { get; set; }

}
