using WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.GetStoresRequest;

namespace WaffarXPartnerApi.Application.Common.DTOs.Valu.ValuRequestDto.PaginationRequest;
public class GetStoresRequestDtoValidator : AbstractValidator<GetStoresRequestDto>
{
    public GetStoresRequestDtoValidator()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageNumberMinLength");
        RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(1).WithMessage("ValuPageSizeMinLength");
        RuleFor(x => x.PageSize).NotNull().LessThanOrEqualTo(50).WithMessage("ValuPageSizeLength");
    }
}
