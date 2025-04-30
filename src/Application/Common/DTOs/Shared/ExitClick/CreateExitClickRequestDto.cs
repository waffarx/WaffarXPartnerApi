namespace WaffarXPartnerApi.Application.Common.DTOs.Shared.ExitClick;
public class CreateExitClickRequestDto
{
    public int CallSource { get; set; }
    public int UserId { get; set; }
    public int ClientApiId { get; set; }
    public string Section { get; set; }
    public Guid AdvertiserGuid { get; set; }
    public int? ProductId { get; set; }
    public string ProductSearchId { get; set; }
    public int? LinkId { get; set; }
    public int ClickSource { get; set; }
    public bool IsMobile { get; set; }
    public string MobileType { get; set; }
    public string XClid { get; set; }
    public Dictionary<string, object> TrackParamKeys { get; set; }
    public bool IsEnglish { get; set; }
    public int? CountryId { get; set; } = 67;
    public string variant { get; set; }
    public string partnerUserIdentifier { get; set; }
    public string subId1 { get; set; }
    public string subId2 { get; set; }
}
