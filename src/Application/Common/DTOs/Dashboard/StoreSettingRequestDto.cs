namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
public class StoreSettingRequestDto
{
    public string StoreId { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
    public string BackgroundColor { get; set; }
}
