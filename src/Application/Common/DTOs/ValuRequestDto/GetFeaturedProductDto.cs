namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class GetFeaturedProductDto
{
    public Guid? ClientApiId { get; set; }
    public bool IsEnglish { get; set; } = true;
    public int? PageNumber { get; set; } = 0;
    public int? Count { get; set; } = 50;
}
