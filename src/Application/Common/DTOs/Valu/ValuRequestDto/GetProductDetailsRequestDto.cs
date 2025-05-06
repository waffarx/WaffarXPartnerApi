namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto;
public class GetProductDetailsRequestDto
{
    public List<string> Products { get; set; }
    public bool IsEnglish { get; set; } = true;
    public Guid ClientApiId { get; set; }
}
