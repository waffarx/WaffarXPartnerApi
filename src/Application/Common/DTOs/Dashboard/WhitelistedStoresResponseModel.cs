namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
public class WhitelistedStoresResponseModel
{
    public int StoreId { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
    public string BackgroundColor { get; set; }
}
