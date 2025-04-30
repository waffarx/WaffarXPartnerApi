namespace WaffarXPartnerApi.Application.Common.DTOs.Shared.ExitClick;
public class ExitClickResponseDto
{
    public string AdvertiserLogo { get; set; }
    public string NoteMessageEn { get; set; }
    public string NoteMessageAr { get; set; }
    public string RedirectMessageEn { get; set; }
    public string RedirectMessageAr { get; set; }
    public string UpToTextEn { get; set; }
    public string UpToTextAr { get; set; }
    public bool IsStoreHaveOptions { get; set; }
    public string ShoppingOptionsMessageEn { get; set; }
    public string ShoppingOptionsMessageAr { get; set; }
    public List<CashBackOptionsDto> CashBackOptions { get; set; }
    public string RedirectUrl { get; set; }
    public bool ShoppingDisabled { get; set; }
    public string ShoppingDisabledMessage { get; set; }
    public ExitClickDutrationDto DurationAlertPopup { get; set; }
}
public class CashBackOptionsDto
{
    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEN { get; set; } = string.Empty;
    public string PercentageText { get; set; } = string.Empty;
    public string ExitClick { get; set; } = string.Empty;
}

public class ExitClickDutrationDto
{
    public string Image { get; set; }
    public string FirstTextEn { get; set; }
    public string FirstTextAr { get; set; }
    public string SecondTextEn { get; set; }
    public string SecondTextAr { get; set; }
}
