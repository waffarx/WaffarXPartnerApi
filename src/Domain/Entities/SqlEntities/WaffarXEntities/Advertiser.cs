
namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class Advertiser
{
    public int Id { get; set; }

    public int? ProviderId { get; set; }

    public int? Advertiserid { get; set; }

    public bool? Accountstatus { get; set; }

    public decimal? Sevendayepc { get; set; }

    public decimal? Threemonthepc { get; set; }

    public string Language { get; set; }

    public string Advertisername { get; set; }

    public string AdvertiserTitle { get; set; }

    public string AdvertisernameAr { get; set; }

    public string Programurl { get; set; }

    /// <summary>
    /// 1 = Joind, 0 = Else
    /// </summary>
    public int? Relationshipstatus { get; set; }

    public bool? Mobiletrackingcertified { get; set; }

    public int? Networkrank { get; set; }

    public bool? Performanceincentives { get; set; }

    public int? Parentcategory { get; set; }

    public string Childcategory { get; set; }

    public string Logo { get; set; }

    public string WaffaeXlogo { get; set; }

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

    public string DeepLinkAndroid { get; set; }

    public string DeepLinkIos { get; set; }

    public string CoverImageUrl { get; set; }

    public string CashbackTerms { get; set; }

    public string CashbackTermsAr { get; set; }

    public string ShoppingSecrets { get; set; }

    public string ShoppingSecretsAr { get; set; }

    public string Countries { get; set; }

    public string Tags { get; set; }

    public bool? IsInternal { get; set; }

    public string SeoDescrEn { get; set; }

    public string SeoDescrAr { get; set; }

    public string KeywordsEn { get; set; }

    public string KeywordsAr { get; set; }

    public string SocialDescrEn { get; set; }

    public string SocialDescrAr { get; set; }

    public string SeoTitleEn { get; set; }

    public string SeoTitleAr { get; set; }

    public string SocialTitleEn { get; set; }

    public string SocialTitleAr { get; set; }

    public string ExitClickTextEn { get; set; }

    public string ExitClickTextAr { get; set; }

    public bool? IsExtension { get; set; }

    /// <summary>
    /// The referrer: (e.g. google, newsletter)
    /// </summary>
    public string XSource { get; set; }

    /// <summary>
    /// Marketing medium: (e.g. cpc, banner, email)
    /// </summary>
    public string XMedium { get; set; }

    /// <summary>
    /// Product, promo code, or slogan (e.g. spring_sale)
    /// </summary>
    public string XName { get; set; }

    /// <summary>
    /// Identify the paid keywords e.g(search terms)
    /// </summary>
    public string XTerm { get; set; }

    /// <summary>
    /// Use to differentiate ads
    /// </summary>
    public string XContent { get; set; }

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

    public decimal? OldPercentagePayout { get; set; }

    public decimal? UserValue { get; set; }

    /// <summary>
    /// 1 = open in app webview, 2 = make user choose, 3 = force to open in external app
    /// </summary>
    public int? StoreRedirectionType { get; set; }

    public bool IsNotified { get; set; }

    public string MobImage { get; set; }

    public string LogoPng { get; set; }

    public bool IsCouponsOnly { get; set; }

    public int? MaxNetworkCommission { get; set; }

    public string ExitClickMsgResource { get; set; }

    public string SolidColor { get; set; }

    public string CautionOfRedirectionPage { get; set; }

    public string CautionOfRedirectionPageAr { get; set; }

    /// <summary>
    /// 1= Online, 2= Instore, 3= Both
    /// </summary>
    public int? AdvertiserType { get; set; }

    public bool IsCashBackConfirmed { get; set; }

    public DateTime? Cdate { get; set; }

    public bool IsMallAdvertiser { get; set; }

    public Guid? StoreGuid { get; set; }
}
