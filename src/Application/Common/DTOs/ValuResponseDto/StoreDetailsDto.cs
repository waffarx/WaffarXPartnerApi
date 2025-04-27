namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class StoreDetailsDto
{
    public StoreDto Store { get; set; }
    public List<ProductSearchResponseModel> StoreFeaturedProducts { get; set; }

}
public class StoreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public string LogoPng { get; set; }
}
