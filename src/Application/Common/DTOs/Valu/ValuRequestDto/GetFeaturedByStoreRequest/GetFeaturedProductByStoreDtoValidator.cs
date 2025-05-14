namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetFeaturedByStoreRequest;
public class GetFeaturedProductByStoreDtoValidator : AbstractValidator<GetFeaturedProductByStoreDto>
{
    public GetFeaturedProductByStoreDtoValidator()
    {
        //RuleFor(x => x.StoreId).NotNull().NotEmpty().WithMessage("ValuStoreId");
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");
        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");
    }
}
