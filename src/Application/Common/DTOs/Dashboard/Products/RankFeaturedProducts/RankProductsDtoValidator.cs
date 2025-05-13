namespace WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Products.RankFeaturedProducts;
public class RankProductsDtoValidator :AbstractValidator<RankProductsDto>
{
    public RankProductsDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");
        RuleFor(x => x.ProductRank)
            .NotEmpty()
            .WithMessage("Product rank is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Product rank must be greater than 0.");
    }
}
