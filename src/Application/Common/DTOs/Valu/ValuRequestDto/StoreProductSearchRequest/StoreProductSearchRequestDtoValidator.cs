namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreProductSearchRequest;
public class StoreProductSearchRequestDtoValidator : AbstractValidator<StoreProductSearchRequestDto>
{
    public StoreProductSearchRequestDtoValidator()
    {
        RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("ValuStoreIdRequired");
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");
        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");

        RuleFor(x => x.MinPrice).NotNull().GreaterThanOrEqualTo(0).WithMessage("ValuMinPriceMinLength");
    }
}
