namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class ProductSearchResultDto
{
    public string Id { get; set; }
    public string MerchantName { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string PriceText { get; set; }
    public string OldPriceText { get; set; }
    public decimal OldPrice { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public string Currency { get; set; }
    public string SKU { get; set; }
    public string Warranty { get; set; }
    public bool Discounted { get; set; }
    public string DiscountedText { get; set; }
    public string PrimaryImg { get; set; }
    public string ErrorImg { get; set; }
    public string ShoppingURL { get; set; }
    public List<string> VariantsImgs { get; set; }
    public Dictionary<string, object> Specification { get; set; }
    public StoreDto Store { get; set; }
    public FeatureDTO Feature { get; set; }
    public List<PriceVariantDto> PriceVariants { get; set; }
    public List<OptionDto> Options { get; set; }
    public List<OffersDto> Offers { get; set; }
}

public class PriceVariantDto
{
    public string VariantId { get; set; }
    public decimal OldPrice { get; set; }
    public int Position { get; set; }
    public string ShoppingURL { get; set; }
    public string ProductUrl { get; set; }
    public double Price { get; set; }
    public string Sku { get; set; }
    public bool Available { get; set; }
    public string Title { get; set; }
    public List<string> Options { get; set; }
}
public class OptionDto
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string ProductId { get; set; }
    public int Position { get; set; }
    public List<string> Values { get; set; }
}
