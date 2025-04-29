
namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class ProductSearchResultWithFiltersDto
{
    public List<ProductSearchResultDto> Products { get; set; }
    public SearchFilterDto Filters { get; set; }

}
public class SearchFilterDto
{
    public List<string> Brands { get; set; }
    public List<StoreDto> Stores { get; set; }
    public List<OfferDto> Offers { get; set; }  
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }

}
public class OfferDto
{
    public string Name { get; set; }
    public string Id { get; set; }
}
