namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.DeleteFeatured;
public class DeleteFeaturedProductDtoValidator : AbstractValidator<DeleteFeaturedProductDto>
{
    public DeleteFeaturedProductDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");
    }
}
