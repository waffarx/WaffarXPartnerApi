namespace WaffarXPartnerApi.Application.Common.DTOs.ValuResponseDto;
public class ProductSearchResponseModel
{
    public string BsonId { get; set; }
    public string ASIN { get; set; }
    public string Id { get; set; }
    public int AdvertiserID { get; set; }
    public string AdvertiserName { get; set; }
    public string MerchantName { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string PriceText { get; set; }
    public string OldPriceText { get; set; }
    public decimal OldPrice { get; set; }
    public string Category { get; set; }
    public List<string> Categories { get; set; }
    public int ReviewsCount { get; set; }
    public string Description { get; set; }
    public FeaturesDTO Features { get; set; }
    public string Brand { get; set; }
    public string Currency { get; set; }
    public string SKU { get; set; }
    public decimal Rate { get; set; }
    public int? RateRateOutOf { get; set; }
    public int? LeftItems { get; set; }
    public bool? FreeShipping { get; set; }
    public string Warranty { get; set; }
    public int CashbackOptionId { get; set; }
    public List<int> CashbackOptionIDs { get; set; }
    public string Language { get; set; }
    public DateTime CDate { get; set; }
    public string UpToText { get; set; }
    public decimal UpTo { get; set; }
    public string WasText { get; set; }
    public decimal Was { get; set; }
    public bool IsHaveWas { get; set; }
    public bool Discounted { get; set; }
    public string DiscountedText { get; set; }
    public string Logo { get; set; }
    public Dictionary<string, string> Specification { get; set; }
    public string PrimaryImg { get; set; }
    public string ErrorImg { get { return "https://waffarxcdn-akcrhqbhbah9gpcc.z01.azurefd.net/waffarx-cdn/img/productsearcherror.png"; } set { } }
    public List<string> VariantsImgs { get; set; }
    public string ExitClickURL { get; set; }
    public double Ellapsed { get; set; }
    public int RelevanceScoreLevi { get; set; }
    public int RelevanceScoreFuzzy { get; set; }
    public List<PriceVariantDTO> price_variants { get; set; }
    public Object options { get; set; }
    public List<OffersDto> Offers { get; set; }

}
public class FeaturesDTO
{
    public List<string> DisplayValues { get; set; }
    public string Label { get; set; }
    public string Locale { get; set; }
}
public class OffersDto
{
    public string OfferId { get; set; }
    public string Name { get; set; }
}
public class PriceVariantDTO
{
    public string variant_id { get; set; }
    public double old_price { get; set; }
    public DateTime created_at { get; set; }
    public int position { get; set; }
    public string product_url { get; set; }
    public double price { get; set; }
    public string product_id { get; set; }
    public string sku { get; set; }
    public bool available { get; set; }
    public string title { get; set; }
    public DateTime updated_at { get; set; }
    public int inventory_quantity { get; set; }
    public List<string> options { get; set; }
}
