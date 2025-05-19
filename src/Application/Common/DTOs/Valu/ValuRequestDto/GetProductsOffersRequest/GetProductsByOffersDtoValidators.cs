namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetProductsOffersRequest;
public class GetProductsByOffersDtoValidators : AbstractValidator<GetProductsByOffersDto>
{
    public GetProductsByOffersDtoValidators()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");
        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");
    }
}
