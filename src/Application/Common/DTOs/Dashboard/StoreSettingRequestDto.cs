namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard;
public class StoreSettingRequestDto
{
    public int StoreId { get; set; }
    public bool IsFeatured { get; set; }
    public int Rank { get; set; }
}
