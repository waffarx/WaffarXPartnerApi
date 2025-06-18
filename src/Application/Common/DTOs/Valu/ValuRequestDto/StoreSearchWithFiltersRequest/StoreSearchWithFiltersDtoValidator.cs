namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.StoreSearchWithFiltersRequest;
public class StoreSearchWithFiltersDtoValidator : AbstractValidator<StoreSearchWithFiltersDto>
{
    public StoreSearchWithFiltersDtoValidator()
    {
        // Add validation rules here if needed
        RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("ValuStoreIdRequired");
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");

        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");

    }
}
