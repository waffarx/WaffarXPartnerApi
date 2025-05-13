namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.UpdateFeatured;
public class UpdateFeaturedProductDtoValidator : AbstractValidator<UpdateFeaturedProductDto>
{
    public UpdateFeaturedProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be less than or equal to end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.");
    }
}
