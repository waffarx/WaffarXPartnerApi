namespace WaffarXPartnerApi.Application.Common.DTOs.ValuRequestDto;
public class GetStoreDto
{
    public Guid StoreId { get; set; }
    public Guid? ClientApiId { get; set; }
    public bool IsEnglish { get; set; } = true;

}
