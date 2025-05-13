
using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
using WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuResponseDto;
public class StoresWithOffersResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public string LogoPng { get; set; }
    public string BackgroundColor { get; set; }
    public List<OfferDto> Offers { get; set; }
    public List<ProductSearchResponseModel> StoreFeaturedProducts { get; set; }
}
