namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferType;
public class OfferTypeRequestDtoValidator : AbstractValidator<OfferTypeRequestDto>
{
    public OfferTypeRequestDtoValidator()
    {
        RuleFor(x => x.NameAr)
        .NotEmpty()
        .WithMessage("Name in Arabic is required.")
        .MinimumLength(2)
        .WithMessage("Name in Arabic must at least 2 characters.");

        RuleFor(x => x.NameEn)
            .NotEmpty()
            .WithMessage("Name in English is required.")
            .MinimumLength(2)
            .WithMessage("Name in English must at least 2 characters.");
    }
}
