namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class Vadvertiser
{
    public int Id { get; set; }

    public int? ProviderId { get; set; }

    public int? Advertiserid { get; set; }

    public string Advertisername { get; set; }

    public string AdvertisernameAr { get; set; }

    public string Programurl { get; set; }

    public string WaffaeXlogo { get; set; }

    public string MobImage { get; set; }

    public string Currency { get; set; }

    public string Description { get; set; }

    public string DescriptionAr { get; set; }

    public decimal? PercentagePayout { get; set; }

    public decimal? FlatPayout { get; set; }

    public string Categories { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public decimal? UserPercentage { get; set; }

    public decimal? WaffarXpercentage { get; set; }

    public string Deeplink { get; set; }

    public string CoverImageUrl { get; set; }

    public string DeepLinkAndroid { get; set; }

    public string DeepLinkIos { get; set; }

    public string CashbackTerms { get; set; }

    public string CashbackTermsAr { get; set; }

    public string ShoppingSecrets { get; set; }

    public string ShoppingSecretsAr { get; set; }

    public string Tags { get; set; }

    public bool? IsInternal { get; set; }

    public bool? IsExtension { get; set; }

    public decimal? MultipleValue { get; set; }

    public DateTime? MultipleFromDate { get; set; }

    public DateTime? MultipleToDate { get; set; }

    public bool? IsMobileIcon { get; set; }

    public bool? IsAndroid { get; set; }

    public bool? IsIos { get; set; }

    public bool? ShoppingDisabled { get; set; }

    public string ShoppingDisabledMsgEn { get; set; }

    public string ShoppingDisabledMsgAr { get; set; }

    public string ShopButton { get; set; }

    public string AdvertiserTitle { get; set; }

    public int? StoreRedirectionType { get; set; }

    public string SolidColor { get; set; }

    public string LogoPng { get; set; }

    public int? AdvertiserType { get; set; }

    public DateTime? Cdate { get; set; }

    public int? CouponsCount { get; set; }

    public int? OrdersCount { get; set; }

    public int? ExitClickCount { get; set; }

    public decimal MaxAdvertiserOption { get; set; }

    public decimal? Was { get; set; }

    public decimal? Upto { get; set; }

    public string CautionOfRedirectionPage { get; set; }

    public string CautionOfRedirectionPageAr { get; set; }

    public Guid? StoreGuid { get; set; }
}
