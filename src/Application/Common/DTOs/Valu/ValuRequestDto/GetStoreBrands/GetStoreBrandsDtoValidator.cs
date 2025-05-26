namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoreBrands;
public class GetStoreBrandsDtoValidator : AbstractValidator<GetStoreBrandsDto>
{
    public GetStoreBrandsDtoValidator()
    {
        RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("ValuStoreIdRequired");

        RuleFor(x => x.SearchText).NotNull().NotEmpty().WithMessage("ValuSearchText");
        RuleFor(x => x.SearchText).MinimumLength(2).WithMessage("ValuBrandSearchTextMinLength");
        RuleFor(x => x.SearchText).MaximumLength(150).WithMessage("ValuSearchTextMaxLength");
    }
}
