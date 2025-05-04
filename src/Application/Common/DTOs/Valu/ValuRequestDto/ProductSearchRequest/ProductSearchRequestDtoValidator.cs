namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.ProductSearchRequest;
public class ProductSearchRequestDtoValidator : AbstractValidator<ProductSearchRequestDto>
{
    public ProductSearchRequestDtoValidator()
    {
        RuleFor(x => x.SearchText).NotNull().NotEmpty().WithMessage("ValuSearchText");
        RuleFor(x => x.SearchText).MinimumLength(3).WithMessage("ValuSearchTextMinLength");
        RuleFor(x => x.SearchText).MaximumLength(150).WithMessage("ValuSearchTextMaxLength");

        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");

        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");
    }
}
