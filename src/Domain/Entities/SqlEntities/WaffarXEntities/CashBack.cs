namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;
/// <summary>
/// Marks the state of the exit click; 0 = Unresolved - 1 = Pending - 2 = Accepted - 3 = Rejected(Cancelled) - 4 = Has Error (Ticket)
/// </summary>
public partial class CashBack
{
    public int Id { get; set; }

    public string Details { get; set; }

    public decimal? OrderValue { get; set; }

    public decimal? NetworkCommision { get; set; }

    public int? StatusId { get; set; }

    public DateTime? Date { get; set; }

    public int? UserId { get; set; }

    public decimal? OrginalValue { get; set; }

    public decimal? ConvertedValue { get; set; }

    public decimal? PendingValue { get; set; }

    public string AdvertiserName { get; set; }

    public int? CashBackTypeId { get; set; }

    public int? ProviderId { get; set; }

    public bool? IsResolved { get; set; }

    public int? AdvertiserId { get; set; }

    public long? ExitClickId { get; set; }

    public long? ConversionId { get; set; }

    public string OrderId { get; set; }

    public long? ApiconversionId { get; set; }

    public string CurrencySymbol { get; set; }

    public string OrderCurrencySymbol { get; set; }

    public DateTime? Cdate { get; set; }

    public string Gclid { get; set; }

    public int? ReferalLogId { get; set; }

    public int? PaymentCycleId { get; set; }

    public decimal? Bonus { get; set; }

    public bool? IsFixed { get; set; }

    public decimal? UserBonusCommission { get; set; }

    public decimal? UserConvertedValue { get; set; }

    public decimal? WaffarXconvertedValue { get; set; }

    public decimal? UserConvertedValueEgp { get; set; }

    public decimal? WaffarXconvertedValueEgp { get; set; }

    public decimal? UserConvertedValueSar { get; set; }

    public decimal? WaffarxConvertedValueSar { get; set; }

    public decimal? OrderValueUsd { get; set; }

    public decimal? OrderValueEgp { get; set; }

    public decimal? OrderValueSar { get; set; }

    public int? CommissionType { get; set; }

    public int? BankId { get; set; }

    public bool? IsEligible { get; set; }

    public bool IsMailSent { get; set; }

    public decimal? ConvertedValueUsd { get; set; }
}
