using WaffarXPartnerApi.Application.Common.DTOs.Valu.SharedModels;

namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class PriceVariantDto
{
    public double OldPrice { get; set; }
    public string ShoppingURL { get; set; }
    public string ShoppingUrlBase { get; set; }
    public double Price { get; set; }
    public bool Available { get; set; }
    public string Title { get; set; }
    public bool Discounted { get; set; }
    public string DiscountedText { get; set; }
    public List<string> Options { get; set; }
    public string PriceText { get; set; }
    public string OldPriceText { get; set; }
}
public class OptionDto
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string ProductId { get; set; }
    public int Position { get; set; }
    public List<string> Values { get; set; }
}

public class BaseProductSearchResultDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string MerchantName { get; set; }
    public decimal Price { get; set; }
    public string PriceText { get; set; }
    public decimal OldPrice { get; set; }
    public string OldPriceText { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
    public string Currency { get; set; }
    public string PrimaryImg { get; set; }
    public string ErrorImg { get; set; }
    public string ShoppingUrl { get; set; }
    public string ShoppingUrlBase { get; set; }
    public List<string> VariantsImgs { get; set; }
    public StoreResponseDto Store { get; set; }
    public List<OfferDto> Offers { get; set; }
}
public class DetailedProductSearchResultDto : BaseProductSearchResultDto
{
    public bool Discounted { get; set; }
    public string DiscountedText { get; set; }
    public string Description { get; set; }
    public string SKU { get; set; }
    public string Warranty { get; set; }
    public Dictionary<string, string> Specification { get; set; }
    public FeatureDTO Feature { get; set; }
    public List<PriceVariantDto> PriceVariants { get; set; }
    public List<OptionDto> Options { get; set; }

}
