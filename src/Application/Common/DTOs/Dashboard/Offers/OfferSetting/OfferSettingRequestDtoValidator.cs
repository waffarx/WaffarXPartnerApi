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

        RuleFor(x => x.IsProductLevel)
            .Must((x, isProductLevel) => isProductLevel || !x.IsStoreLevel)
            .WithMessage("Either Product Level or Store Level must be selected, not both.");

        RuleFor(x => x.IsStoreLevel)
            .Must((x, isStoreLevel) => isStoreLevel || !x.IsProductLevel)
            .WithMessage("Either Product Level or Store Level must be selected, not both.");

        RuleFor(x => x.StoreIds)
            .Must((x, storeIds) => x.IsStoreLevel ? storeIds != null && storeIds.Count > 0 : true)
            .WithMessage("At least one Store ID is required when Store Level is selected.");

        RuleFor(x => x.ProductIds)
            .Must((x, productIds) => x.IsProductLevel ? productIds != null && productIds.Count > 0 : true)
            .WithMessage("At least one Product ID is required.");
    }
}
