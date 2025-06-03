namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Offers.OfferSetting;
public class OfferSettingRequestDtoValidator : AbstractValidator<OfferSettingRequestDto>
{
    public OfferSettingRequestDtoValidator()
    {
        RuleFor(x => x.OfferLookUpId)
            .NotEmpty().WithMessage("Offer LookUp ID is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start Date is required.")
            .LessThan(x => x.EndDate).WithMessage("Start Date must be before End Date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End Date is required.");

        // Check when IsStoreLevel is true, StoreIds must have at least one item
        RuleFor(x => x.StoreIds)
            .NotEmpty()
            .When(x => x.IsStoreLevel)
            .WithMessage("At least one Store ID is required when Store Level is selected.");

        // Check when IsProductLevel is true, ProductIds must have at least one item
        RuleFor(x => x.ProductIds)
            .NotEmpty()
            .When(x => x.IsProductLevel)
            .WithMessage("At least one Product ID is required when Product Level is selected.");
    }
}
