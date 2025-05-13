namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;
public class StoreResponseDto : StoreDto
{
    public string ShoppingUrl { get; set; }
    public string ShoppingUrlBase { get; set; }

}


public class StoreWithOffersResponseDto : StoreDto
{
    public string ShoppingUrl { get; set; }
    public string ShoppingUrlBase { get; set; }
    public List<OfferDto> Offers { get; set; }
}
