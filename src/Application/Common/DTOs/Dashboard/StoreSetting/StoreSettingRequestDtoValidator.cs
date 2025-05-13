namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.StoreSetting;
public class StoreSettingRequestDtoValidator : AbstractValidator<StoreSettingRequestDto>
{
    public StoreSettingRequestDtoValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty()
            .WithMessage("Store ID is required.");
        RuleFor(x => x.IsFeatured)
            .NotNull()
            .WithMessage("IsFeatured is required.");
        RuleFor(x => x.Rank)
            .GreaterThan(-1)
            .WithMessage("Rank must be greater than negative number.");
    }
}
