namespace WaffarXPartnerApi.Application.Common.DTOs.Shared;
public class BaseSharedDto
{
    public bool IsEnglish { get; set; } = true;
    public Guid? ClientApiId { get; set; }
}
