namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

/// <summary>
/// Marks the state of the exit click; 0 = Unresolved - 1 = Pending - 2 = Accepted - 3 = Rejected(Cancelled) - 4 = Has Error (Ticket)
/// </summary>
public partial class ExitClick
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? Cdate { get; set; }

    public int? AdvertiserId { get; set; }

    public int? ProviderId { get; set; }

    public string Deeplink { get; set; }

    public string DeeplinkAndroid { get; set; }

    public string DeeplinkIos { get; set; }

    public string ExitType { get; set; }

    public int? IsResolved { get; set; }

    public bool? HasTransaction { get; set; }

    public int? CategoryId { get; set; }

    public int? ProductId { get; set; }

    public decimal? AdvertiserCommision { get; set; }

    public decimal? AdvertiserPayout { get; set; }

    /// <summary>
    /// 0 =WaffarX, 1 = MobileApiAndroid, 2 = MobileApiIOS, 3 = BrowserExtension, 7 = Client Api
    /// </summary>
    public short? ClickSource { get; set; }

    public bool? IsMobile { get; set; }

    public string MobileType { get; set; }

    public Guid? Xclid { get; set; }

    public string Fbp { get; set; }

    public string Fbc { get; set; }

    public int? BankId { get; set; }

    public decimal? Bonus { get; set; }

    public bool? IsFixed { get; set; }

    public decimal? UserBonusCommission { get; set; }
}
