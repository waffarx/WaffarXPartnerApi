namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreSearchWithFiltersRequest;
public class StoreSearchWithFiltersDto
{
    public Guid StoreId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool SortByPriceDsc { get; set; } = true;
    public ValuStoreSearchFilterDto Filter { get; set; }
}
public class ValuStoreSearchFilterDto
{
    public string Brands { get; set; }
    public string Category { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public bool Discounted { get; set; }
}

public class ValuStoreSearchQueryDto
{
    public int StoreId { get; set; }
    public int PageNumber { get; set; }
    public int ItemCount { get; set; }
    public bool IsEnglish { get; set; } = true;
    public bool SortByPriceDsc { get; set; } = true;
    public Guid ClientApiId { get; set; }
    public ValuStoreSearchFilterDto Filter { get; set; }
}
