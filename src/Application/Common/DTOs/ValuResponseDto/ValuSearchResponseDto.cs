namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class ValuSearchResponseDto
{
    public List<ProductSearchResponseModel> Products { get; set; }
    public SearchFilters Filters { get; set; }

}

public class SearchFilters
{
    public List<string> Brands { get; set; }
    public List<StoreDto> Stores { get; set; }
    public List<OfferDto> Offers { get; set; }  
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
}

