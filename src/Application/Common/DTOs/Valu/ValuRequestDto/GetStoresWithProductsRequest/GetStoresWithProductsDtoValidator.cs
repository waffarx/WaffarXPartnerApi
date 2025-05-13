namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresWithProductsRequest;
public class GetStoresWithProductsDtoValidator : AbstractValidator<GetStoresWithProductsDto>
{
    public GetStoresWithProductsDtoValidator()
    {
        RuleFor(x => x.ProductsCount).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuProductsCount");
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");
        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");
    }
}
